using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Database.Search;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils.Misc;
using Listen.Api.Utils.Search;
using TagLib;
using File = System.IO.File;

namespace Listen.Api.Utils.BookUtils
{
    public static class UpdateBooks
    {
        private static Task UpdateTask { get; set; }
        private static DateTime UpdateTime { get; set; }

        public static void UpdateBooksFolder()
        {
            if (UpdateTime >= DateTime.Now.AddMinutes(-5) || UpdateTask?.Status == TaskStatus.Running) return;

            UpdateTask = new Task(() =>
            {
                var context = new UserContext();
                UpdateAudioFiles(context);
                var audioFiles = context.GetAll<AudioFile>(UserContext.ReadType.Shallow);
                var folders = audioFiles.Select(d => d.Folder).Distinct().ToList();
                var books = context.GetAll<Book>(UserContext.ReadType.Shallow).ToList();
                DeleteRemovedBooks(context, books, folders);
                AddMissingBooks(context, books, folders, audioFiles);
            });
            UpdateTime = DateTime.Now;
            UpdateTask.Start();
        }

        private static void UpdateAudioFiles(UserContext context)
        {
            var settings = SettingsUtil.Settings;
            if (settings?.Path == null)
            {
                throw new Exception("<Path> setting has not been set");
            }

            var allFiles = FindAudioFiles(settings.Path);

            var index = 0;
            const int step = 100;
            while (index < allFiles.Count)
            {
                var selectedFiles = allFiles.Skip(index).Take(step).ToList();
                index += step;

                var encodedpaths = selectedFiles.Select(d => d.EncodedPath).ToArray();
                var oldFiles = context.Search(
                    new SearchObject<AudioFile>()
                    .Add(SearchOperationType.MatchMultiple, "EncodedPath", encodedpaths),
                    UserContext.ReadType.Shallow);
                var added = selectedFiles.Where(d => oldFiles.All(e => e.FilePath != d.FilePath)).ToList();
                var deleted = oldFiles.Where(d => selectedFiles.All(e => e.FilePath != d.FilePath)).ToList();

                added.ForEach(d => context.AddDefault(d));
                deleted.ForEach(d => context.Remove<AudioFile>(d.Id));
            }

        }

        private static void AddMissingBooks(UserContext context, List<Book> books, List<string> folders, AudioFile[] audioFiles)
        {
            var toAdd = folders.Where(d => books.All(e => e.Path != d)).ToList();
            var newBooks = toAdd.Select(folder => CreateNewBook(context, folder, audioFiles)).ToList();
            newBooks.ForEach(newBook => context.AddDefault(newBook));
            books.AddRange(newBooks);
        }

        private static Book CreateNewBook(UserContext context, string folder, AudioFile[] audioFiles)
        {
            var selectedAudioFiles = audioFiles.Where(d => d.Folder == folder).ToArray();
            var idFilePath = Path.Combine(folder, "bookid.listen");
            if (File.Exists(idFilePath))
            {
                var fileId = File.ReadAllText(idFilePath);
                var id = new Id(fileId);
                if (id.IsIdentifierForType<Book>())
                {
                    var existing = context.Get<Book>(id, UserContext.ReadType.Shallow);
                    if (existing != null)
                    {
                        var newBook = new Book(
                            existing.Title,
                            existing.Author,
                            folder,
                            existing.CoverImage,
                            BookState.Manual,
                            selectedAudioFiles);
                        context.UpdateDefault(newBook, existing.Id);
                        return newBook;
                    }
                }
            }
            var ret = new Book(
                null,
                null,
                folder,
                MiscUtils.Get404Image(),
                BookState.New,
                selectedAudioFiles);
            File.WriteAllText(idFilePath, ret.Id.ToString());
            return ret;
        }

        private static void DeleteRemovedBooks(UserContext context, List<Model.Book> books, List<string> folders)
        {
            var toDelete = books.Where(d => !folders.Contains(d.Path)).ToList();
            toDelete.ForEach(book =>
            {
                var deletedBook =
                    new Model.Book(
                        book.Title,
                        book.Author,
                        null,
                        book.CoverImage,
                        BookState.Deleted,
                        new AudioFile[0]);
                context.UpdateDefault(deletedBook, book.Id);
            });
            books.RemoveAll(d => d.State == (int)BookState.Deleted);
        }

        private static List<AudioFile> FindAudioFiles(string path, bool shallow = false)
        {
            var ret = new List<AudioFile>();
            var files = Directory.GetFiles(path);
            var audiofiles = files.Select(d => new AudioFile(d)).Where(d => d.MimeType?.StartsWith("audio") ?? false);

            ret.AddRange(audiofiles);
            if (shallow) return ret;
            foreach (var subDirectory in Directory.GetDirectories(path))
            {
                ret.AddRange(FindAudioFiles(subDirectory));
            }

            return ret;
        }

        public static Book LookupBook(Book book)
        {
            var context = new UserContext();

            var firstFile = context.Get<AudioFile>(book.AudioFiles.First().Id, UserContext.ReadType.Shallow);
            string album;
            string title;
            string author;
            var folderSearchString = Path.GetFileName(book.Path);
            using (var readStream = new FileStream(firstFile.FilePath, FileMode.Open, FileAccess.Read))
            {
                var tagFile = TagLib.File.Create(new StreamFileAbstraction(firstFile.FilePath, readStream, null));
                album = tagFile.Tag.Album;
                author = tagFile.Tag.FirstAlbumArtist;
                title = tagFile.Tag.Title;
            }
            var tagSearchString1 = $"{author} {title}";
            var tagSearchString2 = $"{author} {album}";

            var results = BigBookSearch.Search(folderSearchString, tagSearchString1, tagSearchString2);
            var bestResult = results.OrderByDescending(d => d.Number).FirstOrDefault(d => d.Author != null && d.Title != null);
            if (bestResult == null || bestResult.Number < SettingsUtil.Settings.AutoMatchThreshold)
            {
                var failedBook = new Book(
                    book.Title,
                    book.Author,
                    book.Path,
                    book.CoverImage,
                    BookState.Failed,
                    book.AudioFiles);
                context.UpdateDefault(failedBook, book.Id);
                return failedBook;
            }

            var existingImage =
                context.Search<CoverImage>("Url", bestResult.ImageLink, UserContext.ReadType.Shallow);
            CoverImage image;
            if (existingImage.Length > 0)
            {
                image = existingImage.First();
            }
            else
            {
                image = new CoverImage(bestResult.ImageLink, MiscUtils.DownloadImage(bestResult.ImageLink));
                context.AddDefault(image);
            }

            var newBook = new Book(
                bestResult.Title,
                bestResult.Author,
                book.Path,
                image,
                BookState.Auto,
                book.AudioFiles);

            context.UpdateDefault(newBook, book.Id);
            return newBook;
        }
    }
}

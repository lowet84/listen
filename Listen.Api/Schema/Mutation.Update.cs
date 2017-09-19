using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Schema.Output;
using Listen.Api.Utils;
using TagLib;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        [Description("Find files and update file list")]
        public DefaultResult<UpdateFileChangesOutput> UpdateFileChanges(UserContext context)
        {
            var oldFiles = context.Search<AudioFile>("FilePath", "", UserContext.ReadType.Shallow) ?? new AudioFile[0];
            var settings = context.Search<Settings>("Path", "", UserContext.ReadType.Shallow).FirstOrDefault();
            if (settings?.Path == null)
            {
                return null;
            }

            var files = FindAudioFiles(settings.Path);
            var added = files.Where(d => oldFiles.All(e => e.FilePath != d.FilePath)).ToList();
            var deleted = oldFiles.Where(d => files.All(e => e.FilePath != d.FilePath)).ToList();

            added.ForEach(d => context.AddDefault(d));
            deleted.ForEach(d => context.Remove<AudioFile>(d.Id));

            var ret = new UpdateFileChangesOutput(files.Count, added.Count, deleted.Count);
            return new DefaultResult<UpdateFileChangesOutput>(ret);
        }

        private (Book, bool) LookupBook(string folder)
        {
            var context = new UserContext();

            var existingBook = context.Search<Book>("Path", folder, UserContext.ReadType.Shallow).FirstOrDefault();
            if (existingBook != null) return (existingBook, false);

            var folderSearchString = Path.GetFileName(folder);

            var firstFile = FindAudioFiles(folder, true).First();
            string album;
            string title;
            string author;
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
            var bestResult = results.OrderByDescending(d => d.Number).FirstOrDefault();
            Book book;
            if (bestResult == null || bestResult.Number < SettingsUtil.Settings.AutoMatchThreshold)
            {
                book = new Book(null, null, folder, MiscUtils.Get404Image(), true);
            }
            else
            {
                var existingImage =
                    context.Search<CoverImage>("Url", bestResult?.ImageLink, UserContext.ReadType.Shallow);
                CoverImage image;
                if (existingImage.Length > 0)
                {
                    image = existingImage.First();
                }
                else
                {
                    image = new CoverImage(bestResult?.ImageLink, MiscUtils.DownloadImage(bestResult?.ImageLink));
                    context.AddDefault(image);
                }

                book = new Book(bestResult?.Title, bestResult?.Author, folder, image, false);
            }

            return (book, true);
        }

        public DefaultResult<LookupBooksOutput> LookupBooks(UserContext context)
        {
            var audioFiles = context.Search<AudioFile>("FilePath", "", UserContext.ReadType.Shallow);
            if (audioFiles == null) return null;
            var folders = audioFiles.Select(d => d.Folder).Distinct().ToList();
            var books = folders.Select(LookupBook).ToList();

            return null;
        }



        private static List<AudioFile> FindAudioFiles(string path, bool shallow = false)
        {
            var ret = new List<AudioFile>();
            var files = Directory.GetFiles(path);
            var audiofiles = files.Select(d => new AudioFile(d)).Where(d => d.MimeType.StartsWith("audio"));

            ret.AddRange(audiofiles);
            if (shallow) return ret;
            foreach (var subDirectory in Directory.GetDirectories(path))
            {
                ret.AddRange(FindAudioFiles(subDirectory));
            }

            return ret;
        }
    }
}

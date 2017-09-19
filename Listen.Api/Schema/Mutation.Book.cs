using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Database.Search;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils;
using RethinkDb.Driver.Ast;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        public DefaultResult<Book> EditBook(
            UserContext context,
            Id bookId,
            string title,
            string author,
            Id? imageId)
        {
            var oldBook = context.Get<Book>(bookId, UserContext.ReadType.Shallow);
            if (oldBook == null)
                return null;
            var image = oldBook.CoverImage;
            if (imageId != null)
            {
                var remoteImage = context.Get<RemoteImage>(imageId.Value, UserContext.ReadType.Shallow);
                if(remoteImage == null)
                    throw new Exception("Id does not match any image");
                var imageContext = new UserContext("query{allImages{id}}");
                var newCoverImage =
                    imageContext.Search<CoverImage>("EncodedUrl", remoteImage.EncodedUrl, UserContext.ReadType.WithDocument)
                    .FirstOrDefault();
                if (newCoverImage == null)
                {
                    newCoverImage = new CoverImage(remoteImage.Url,MiscUtils.DownloadImage(remoteImage.Url));
                    context.AddDefault(newCoverImage);
                }
                image = newCoverImage;
            }
            var newBook = new Book(
                title ?? oldBook.Title,
                author ?? oldBook.Author,
                oldBook.Path,
                image,
                false);

            context.UpdateDefault(newBook, oldBook.Id);
            return new DefaultResult<Book>(newBook);
        }

        public DefaultResult<RemoteImageCollection> SearchForCovers(
            UserContext context,
            string searchString)
        {
            var results = BigBookSearch.SearchBigBookSearch(searchString);
            var encodedUrls = results.Select(d => Convert.ToBase64String(Encoding.UTF8.GetBytes(d.Img)))
                .ToArray();
            var existing = context.Search(
                new SearchObject<RemoteImage>()
                    .Add(SearchOperationType.MatchMultiple, "EncodedUrl", encodedUrls)
                , UserContext.ReadType.Shallow);

            var newImages = results
                .Where(d => existing.All(e => e.Url != d.Img))
                .Select(d => new RemoteImage(d.Img))
                .ToList();
            newImages.ForEach(d => context.AddDefault(d));

            return new DefaultResult<RemoteImageCollection>(new RemoteImageCollection(newImages.Union(existing).ToArray()));
        }
    }
}

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Listen.Api.Utils.Search
{
    public static class BigBookSearch
    {
        public static BookSearchResult[] Search(params string[] searchStrings)
        {
            var bigBookSearchTasks = searchStrings.Select(d => new Task<BookResult[]>(() => SearchBigBookSearch(d))).ToArray();
            bigBookSearchTasks.ToList().ForEach(d => d.Start());
            Task.WaitAll(bigBookSearchTasks);
            var results = bigBookSearchTasks.SelectMany(d => d.Result).Where(d => d != null).ToList();
            var top = results.Where(d => d.Index <= 2).ToList();
            var amazonTasks = top.Select(d => new Task<AmazonResult>(() => SearchAmazon(d))).ToArray();
            amazonTasks.ToList().ForEach(d => d.Start());
            Task.WaitAll(amazonTasks);
            var amazonResults = amazonTasks.Select(d => d.Result).Where(d => d != null).OrderByDescending(d => d.Number).ToList();
            var ret = amazonResults.Select(CreateBookSearchResult).ToArray();
            return ret;
        }

        public static BookResult[] SearchBigBookSearch(string searchString)
        {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
            {
                var url =
                    $"http://bigbooksearch.com/query.php?SearchIndex=books&Keywords={searchString}&ItemPage=1";
                var html = client
                    .GetStringAsync(url)
                    .Result;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var images = doc.DocumentNode.Descendants("img")
                    .Select(d => d.Attributes.FirstOrDefault(e => e.Name == "src")?.Value).ToList();
                var links = doc.DocumentNode.Descendants("a")
                    .Select(d => d.Attributes.FirstOrDefault(e => e.Name == "href")?.Value).ToList();

                if (images.Count != links.Count) return null;
                var result = Enumerable.Range(0, images.Count)
                    .Select(d => new BookResult { Img = images[d], Link = links[d], Index = d }).ToList();

                return result.ToArray();
            }
        }

        private static AmazonResult SearchAmazon(BookResult bookResult)
        {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
            {
                var url = bookResult.Link;
                var html = client
                    .GetStringAsync(url)
                    .Result;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var reviews = doc.GetElementbyId("acrCustomerReviewText");
                if (reviews == null) return null;
                var numberText = reviews.InnerText.Replace(" customer reviews", string.Empty)
                    .Replace(",", string.Empty);
                int.TryParse(numberText, out var number);
                number = number / (1 + bookResult.Index);
                return new AmazonResult { BookResult = bookResult, Number = number, HtmlDocument = doc };
            }
        }

        public class BookResult
        {
            public string Link { get; set; }
            public string Img { get; set; }
            public int Index { get; set; }
        }

        private class AmazonResult
        {
            public BookResult BookResult { get; set; }
            public int Number { get; set; }
            public HtmlDocument HtmlDocument { get; set; }
        }

        private static BookSearchResult CreateBookSearchResult(AmazonResult amazonResult)
        {
            var title = amazonResult.HtmlDocument.GetElementbyId("productTitle").InnerText;
            var author = amazonResult.HtmlDocument.DocumentNode
                .Descendants("a"
                ).FirstOrDefault(d => d.Attributes.Contains("class")
                    &&
                    d.Attributes["class"].Value.Contains("contributorNameID"))?.InnerText;
            return new BookSearchResult(title, author, amazonResult.BookResult.Img, amazonResult.Number);
        }

        public class BookSearchResult
        {
            public string Title { get; }
            public string Author { get; }
            public string ImageLink { get; }
            public int Number { get; }

            public BookSearchResult(string title, string author, string imageLink, int number)
            {
                Title = title;
                Author = author;
                ImageLink = imageLink;
                Number = number;
            }
        }
    }
}

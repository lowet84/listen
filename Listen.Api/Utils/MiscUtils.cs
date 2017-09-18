using System;
using System.Linq;
using System.Net.Http;
using GraphQlRethinkDbLibrary;
using Listen.Api.Model;

namespace Listen.Api.Utils
{
    public static class MiscUtils
    {
        public static string DownloadImage(string url)
        {
            if (url == null) return null;

            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
            {
                var data = client.GetByteArrayAsync(url).Result;
                return Convert.ToBase64String(data);
            }
        }

        public static CoverImage Get404Image()
        {
            var context = new UserContext();
            const string url = "https://images-na.ssl-images-amazon.com/images/I/61ztm36SbiL.jpg";
            var existing = context
                .Search<CoverImage>("Url", url, UserContext.ReadType.Shallow);
            if (existing.Length > 0) return existing.First();
            var image = new CoverImage(url, DownloadImage(url));
            context.AddDefault(image);
            return image;
        }
    }
}

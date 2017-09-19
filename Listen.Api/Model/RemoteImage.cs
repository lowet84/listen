using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQlRethinkDbLibrary.Schema.Types;
using HtmlAgilityPack;
using Listen.Api.Utils;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class RemoteImage : NodeBase<RemoteImage>, IDefaultImage
    {
        public string EncodedUrl { get; }

        public string Url { get; }

        public RemoteImage(string url)
        {
            EncodedUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
            Url = url;
        }

        [JsonIgnore]
        public string ContentType
        {
            get
            {
                new FileExtensionContentTypeProvider().TryGetContentType(Url, out var contentType);
                return contentType;
            }
        }

        [JsonIgnore]
        public byte[] ImageData
        {
            get
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
                {
                    return client.GetByteArrayAsync(Url).Result;
                }
            }
        }
    }

    public class RemoteImageCollection
    {
        public RemoteImage[] Images { get; }

        public RemoteImageCollection(RemoteImage[] images)
        {
            Images = images;
        }
    }
}

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

        public RemoteImage(string url)
        {
            EncodedUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
        }

        [JsonIgnore]
        public string Url => Encoding.UTF8.GetString(Convert.FromBase64String(EncodedUrl));

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
}

using System;
using System.Text;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQlRethinkDbLibrary.Schema.Types;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class CoverImage : NodeBase<CoverImage>, IDefaultImage
    {
        public string Data { get; }

        public string EncodedUrl { get; }

        public string Url { get; }

        public CoverImage(string url, string data)
        {
            Data = data;
            Url = url;
            EncodedUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
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
        public byte[] ImageData => Convert.FromBase64String(Data);
    }
}

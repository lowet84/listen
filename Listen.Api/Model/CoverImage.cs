using System;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQlRethinkDbLibrary.Schema.Types;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class CoverImage : NodeBase<CoverImage>, IDefaultImage
    {
        public string Url { get; }
        public string Data { get; }

        public CoverImage(string url, string data)
        {
            Url = url;
            Data = data;
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

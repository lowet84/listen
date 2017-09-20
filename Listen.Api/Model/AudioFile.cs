using System;
using System.IO;
using System.Text;
using GraphQlRethinkDbLibrary.Schema.Types;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Listen.Api.Model
{
    public class AudioFile : NodeBase<AudioFile>
    {
        public string EncodedPath { get; }

        public AudioFile(string filePath)
        {
            EncodedPath = Convert.ToBase64String(Encoding.UTF8.GetBytes(filePath));
        }

        [JsonIgnore]
        public string FilePath => EncodedPath != null ? Encoding.UTF8.GetString(Convert.FromBase64String(EncodedPath)) : null;

        [JsonIgnore]
        public string Folder => Path.GetDirectoryName(FilePath);

        [JsonIgnore]
        public string FileName => Path.GetFileName(FilePath);

        [JsonIgnore]
        public string MimeType
        {
            get
            {
                new FileExtensionContentTypeProvider().TryGetContentType(FilePath, out var contentType);
                return contentType;
            }
        }

        public override bool Equals(object obj)
        {
            var file = obj as AudioFile;
            return file?.FilePath == FilePath;
        }
    }
}

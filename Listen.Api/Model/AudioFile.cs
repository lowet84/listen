using System.IO;
using GraphQlRethinkDbLibrary.Schema.Types;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class AudioFile : NodeBase<AudioFile>
    {
        public string FilePath { get; }

        public AudioFile(string filePath)
        {
            FilePath = filePath;
        }


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

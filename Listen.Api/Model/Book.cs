using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Schema.Types;
using Listen.Api.Utils;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class Book : NodeBase<Book>
    {
        public string Title { get; }
        public string Author { get; }
        public string EncodedPath { get; }

        [JsonIgnore]
        public string Path => Encoding.UTF8.GetString(Convert.FromBase64String(EncodedPath));
        public CoverImage CoverImage { get; }
        public int Failed { get; set; }

        public Book(string title, string author, string path, CoverImage coverImage, bool failed)
        {
            Title = title;
            Author = author;
            EncodedPath = Convert.ToBase64String(Encoding.UTF8.GetBytes(path));
            CoverImage = coverImage;
            Failed = failed ? 1 : 0;
        }
    }
}

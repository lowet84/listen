using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Schema.Types;
using Listen.Api.Utils;

namespace Listen.Api.Model
{
    public class Book : NodeBase<Book>
    {
        public string Title { get; }
        public string Author { get; }
        public string Path { get; }
        public CoverImage CoverImage { get; }
        public int Failed { get; set; }

        public Book(string title, string author, string path, CoverImage coverImage, bool failed)
        {
            Title = title;
            Author = author;
            Path = path;
            CoverImage = coverImage;
            Failed = failed ? 1 : 0;
        }
    }
}

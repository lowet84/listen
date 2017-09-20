using System;
using System.Text;
using GraphQlRethinkDbLibrary.Schema.Types;
using Newtonsoft.Json;

namespace Listen.Api.Model
{
    public class Book : NodeBase<Book>
    {
        public string Title { get; }
        public string Author { get; }
        public string EncodedPath { get; }
        public AudioFile[] AudioFiles { get; }

        [JsonIgnore]
        public string Path => EncodedPath == null ? null : Encoding.UTF8.GetString(Convert.FromBase64String(EncodedPath));
        public CoverImage CoverImage { get; }
        public int State { get; set; }

        [JsonIgnore]
        public BookState BookState => (BookState)State;

        public Book(string title, string author, string path, CoverImage coverImage, BookState state, AudioFile[] audioFiles)
        {
            Title = title;
            Author = author;
            EncodedPath = path != null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(path)) : null;
            CoverImage = coverImage;
            AudioFiles = audioFiles;
            State = (int)state;
        }
    }

    public enum BookState
    {
        New = 0,
        Auto = 1,
        Failed = 2,
        Manual = 3,
        Deleted = 4
    }
}

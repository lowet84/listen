using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Schema.Types;

namespace Listen.Api.Model
{
    public class CoverImage : NodeBase<CoverImage>
    {
        public string Url { get; }
        public string Data { get; }

        public CoverImage(string url, string data)
        {
            Url = url;
            Data = data;
        }
    }
}

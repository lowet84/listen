using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Handlers
{
    public class ImageHandler : DefaultImageHandler
    {
        public override IDefaultImage GetImage(string key)
        {
            return new UserContext().Get<CoverImage>(new Id(key), UserContext.ReadType.Shallow);
        }
    }
}

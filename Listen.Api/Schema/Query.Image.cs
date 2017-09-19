using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        public Task<CoverImage[]> AllImages(UserContext context)
        {
            return Task.FromResult(context.GetAll<CoverImage>(UserContext.ReadType.WithDocument));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        [Description("Get all books")]
        public Task<Book[]> AllBooks(UserContext context)
        {
            var books = context.Search<Book>("id", "");
            return Task.FromResult(books);
        }

        [Description("Get book by id")]
        public Task<Book> Book(UserContext context, Id id)
        {
            return Task.FromResult(context.Get<Book>(id));
        }
    }
}

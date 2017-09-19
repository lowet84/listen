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
        public Book[] AllBooks(UserContext context)
        {
            var books = context.Search<Book>("id", "", UserContext.ReadType.WithDocument);
            return books;
        }

        [Description("Get book by id")]
        public Book Book(UserContext context, Id id)
        {
            return context.Get<Book>(id);
        }
    }
}

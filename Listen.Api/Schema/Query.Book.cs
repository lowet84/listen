﻿using GraphQlRethinkDbLibrary;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        [Description("Get all books")]
        public Book[] AllBooks(UserContext context)
        {
            UserUtil.IsAuthorized(context, UserType.Admin, UserType.Normal);
            var books = context.Search<Book>("id", "", UserContext.ReadType.WithDocument);
            return books;
        }

        [Description("Get book by id")]
        public Book Book(UserContext context, Id id)
        {
            UserUtil.IsAuthorized(context, UserType.Admin, UserType.Normal);
            return context.Get<Book>(id);
        }
    }
}

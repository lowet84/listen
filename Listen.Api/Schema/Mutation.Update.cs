﻿using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Schema.Output;
using Listen.Api.Utils.BookUtils;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        [Description("Find files and update file list")]
        public DefaultResult<string> UpdateFileChanges(UserContext context)
        {
            UpdateBooks.UpdateBooksFolder();
            return new DefaultResult<string>("done");
        }

        [Description("Look up book title, author and image")]
        public DefaultResult<Book> LookupBook(UserContext context, Id id)
        {
            var book = context.Get<Book>(id, UserContext.ReadType.Shallow);
            var ret = UpdateBooks.LookupBook(book);
            return new DefaultResult<Book>(ret);
        }
    }
}

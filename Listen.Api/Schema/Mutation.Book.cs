using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils;
using RethinkDb.Driver.Ast;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        public DefaultResult<string> EditBook(
            UserContext context,
            Id id,
            string title,
            string author,
            Id imageId)
        {
            return null;
        }

        //public DefaultResult<RemoteImage> SearchForCovers(
        //    UserContext context, 
        //    string searchString)
        //{
        //    var results = BigBookSearch.Search(searchString);

        //}
    }
}

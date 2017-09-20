using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        [Description("The app settings")]
        public Settings Settings(UserContext context)
        {
            var settings = context.Search<Settings>("id", "", UserContext.ReadType.WithDocument).FirstOrDefault();
            return settings;
        }
    }
}

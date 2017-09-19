using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        [Description("The app settings")]
        public Task<Settings> Settings(UserContext context)
        {
            var settings = context.Search<Settings>("id", "", UserContext.ReadType.WithDocument).FirstOrDefault();
            return Task.FromResult(settings);
        }
    }
}

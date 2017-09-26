using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        [Description("The app settings")]
        public Settings Settings(UserContext context)
        {
            UserUtil.IsAuthorized(context, UserType.Normal);
            var settings = context.Search<Settings>("id", "", UserContext.ReadType.WithDocument).FirstOrDefault();
            return settings;
        }
    }
}

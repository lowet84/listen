using GraphQlRethinkDbLibrary;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        public CoverImage[] AllImages(UserContext context)
        {
            UserUtil.IsAuthorized(context, UserType.Admin, UserType.Normal);
            return context.GetAll<CoverImage>(UserContext.ReadType.WithDocument);
        }
    }
}

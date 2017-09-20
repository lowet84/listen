using GraphQlRethinkDbLibrary;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        public CoverImage[] AllImages(UserContext context)
        {
            return context.GetAll<CoverImage>(UserContext.ReadType.WithDocument);
        }
    }
}

using System;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace Listen.Api.Handlers
{
    public class ImageHandler : DefaultImageHandler
    {
        public override Task Process(HttpContext context)
        {
            var userKey = TokenUtil.GetUserKey(context);
            var user = UserUtil.GetUser(userKey);
            if(user == null)
                throw new Exception("Unauthorized");
            return base.Process(context);
        }

        public override IDefaultImage GetImage(string key)
        {
            var id = new Id(key);

            if (id.IsIdentifierForType<CoverImage>())
            {
                return new UserContext().Get<CoverImage>(id, UserContext.ReadType.Shallow);

            }
            if (id.IsIdentifierForType<RemoteImage>())
            {
                return new UserContext().Get<RemoteImage>(id, UserContext.ReadType.Shallow);
            }

            throw new Exception("Id must be of type CoverImage or RemoteImage");
        }
    }
}

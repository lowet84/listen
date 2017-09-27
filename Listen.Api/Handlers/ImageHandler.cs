using System;
using System.Linq;
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
        public override IDefaultImage GetImage(string key)
        {
            var split = key.Split("___");

            User user;
            if (split.Length == 1 && Environment.GetEnvironmentVariable("DEBUG_USER") == "true")
            {
                user = UserUtil.GetUser(UserUtil.DebugUserNameAndKey);
            }
            else
            {
                var token = split[1];
                user = UserUtil.GetUser(TokenUtil.GetUserKey(token));
            }

            var id = new Id(split[0]);
            
            var allowdUserTypes = new[] {UserType.Normal, UserType.Admin};
            if (!allowdUserTypes.Contains((UserType) user.UserType))
            {
                throw new Exception("Unauthorized");
            }

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

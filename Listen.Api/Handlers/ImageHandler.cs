using System;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Handlers;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Handlers
{
    public class ImageHandler : DefaultImageHandler
    {
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

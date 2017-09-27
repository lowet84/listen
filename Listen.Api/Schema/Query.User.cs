using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;

namespace Listen.Api.Schema
{
    public partial class Query
    {
        public DefaultResult<bool> IsAuthenticated(UserContext context)
        {
            var user = UserUtil.GetUser(context.UserName);
            if (user == null) return new DefaultResult<bool>(false);
            var allowedUserTypes = new[] {UserType.Admin, UserType.Normal};
            return new DefaultResult<bool>(allowedUserTypes.Contains((UserType) user.UserType));
        }

        public User[] AllUsers(UserContext context)
        {
            UserUtil.IsAuthorized(context, UserType.Admin);
            return context.GetAll<User>(UserContext.ReadType.WithDocument);
        }

        public User User(UserContext context, Id id)
        {
            UserUtil.IsAuthorized(context, UserType.Admin);
            return context.Get<User>(id);
        }

        public User MyUser(UserContext context)
        {
            var user = UserUtil.GetUser(context.UserName);
            return user;
        }

        public DefaultResult<string> GetApplyingUsername(UserContext context)
        {
            if (context.UserName == null)
                return null;
            var userResult = context.Search<User>(
                d => d.Filter(user => user.G("UserKey").Eq(context.UserName).And(user.G("UserType").Eq(2))),
                UserContext.ReadType.Shallow);
            return new DefaultResult<string>(userResult.FirstOrDefault()?.UserName ?? string.Empty);
        }
    }
}

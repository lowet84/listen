using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using Listen.Api.Model;
using Listen.Api.Utils.UserUtils;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        public DefaultResult<User> AddFirstUser(
            UserContext userContext,
            string userName)
        {
            if(!UserUtil.IsFirstUser(userContext.UserName))
                throw new Exception("Listen! has already been set up with first user");
            if (userContext.UserName == null)
                throw new Exception("No user key set.");

            var user = new User(userName, userContext.UserName, UserType.Admin);
            userContext.AddDefault(user);
            return new DefaultResult<User>(user);
        }

        public DefaultResult<User> ApplyForLogin(UserContext context, string userName)
        {
            if (context.UserName == null)
                return new DefaultResult<User>(null);
            var userResult = context.Search<User>(
                d => d.Filter(user => user.G("UserKey").Eq(context.UserName).And(user.G("UserType").Eq(2))),
                UserContext.ReadType.Shallow);
            var existing = userResult.FirstOrDefault();

            var newUser = new User(userName,context.UserName,UserType.Pending);
            if (existing != null)
            {
                context.UpdateDefault(newUser, existing.Id);
            }
            else
            {
                context.AddDefault(newUser);
            }
            return new DefaultResult<User>(newUser);
        }
    }
}

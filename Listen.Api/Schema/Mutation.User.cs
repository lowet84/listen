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
    }
}

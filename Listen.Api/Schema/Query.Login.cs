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
    public partial class Query
    {
        public DefaultResult<bool> IsFirstLogin(UserContext userContext)
        {
            return new DefaultResult<bool>(UserUtil.IsFirstUser(userContext.UserName));
        }

        public Login LoginOptions(UserContext context)
        {
            return new Login();
        }
    }
}

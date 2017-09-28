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
        public Login LoginOptions(UserContext context)
        {
            return new Login();
        }

        public LoginStatus GetLoginStatus(UserContext context)
        {
            if (UserUtil.IsFirstUser(context.UserName))
                return LoginStatus.FirstLogin;
            if (context.UserName == null)
                return LoginStatus.NotLoggedIn;
            var user = UserUtil.GetUser(context.UserName);
            if (user == null || user.UserType == (int)UserType.Pending)
                return LoginStatus.Apply;
            if (user.UserType == (int)UserType.Rejected)
                return LoginStatus.Rejected;
            if (user.UserType == (int)UserType.Admin || user.UserType == (int)UserType.Normal)
                return LoginStatus.Ok;
            return LoginStatus.Error;
        }

        public enum LoginStatus
        {
            FirstLogin,
            Error,
            NotLoggedIn,
            Apply,
            Rejected,
            Ok
        }
    }
}

using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
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
            var ret = context.GetAll<User>(UserContext.ReadType.WithDocument);
            ret = ret.Where(d => d.UserKey != UserUtil.DebugUserNameAndKey).ToArray();
            return ret;
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
                d => d.Filter(user => user.G("UserKey").Eq(context.UserName)),
                UserContext.ReadType.Shallow);
            var applyingUser = userResult.SingleOrDefault();
            if(applyingUser == null)
                return new DefaultResult<string>(string.Empty);
            if(applyingUser.UserType == (int)UserType.Rejected)
                return new DefaultResult<string>("Rejected");
            if (applyingUser.UserType == (int)UserType.Pending)
                return new DefaultResult<string>(applyingUser.UserName ?? string.Empty);
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using Listen.Api.Model;

namespace Listen.Api.Utils.UserUtils
{
    public static class UserUtil
    {
        public const string DebugUserNameAndKey = "Debug";

        public static void IsAuthorized(UserContext context, params UserType[] userTypes)
        {
            var user = GetUser(context.UserName);
            if (user == null || !userTypes.Contains((UserType)user.UserType))
                throw new Exception("Unauthorized");
        }

        public static User GetUser(string userKey)
        {
            var debug = Environment.GetEnvironmentVariable("DEBUG_USER") == "true";
            if (debug && userKey == null) return GetDebugUser();

            var existing = new UserContext().Search<User>(
                "UserKey", userKey, UserContext.ReadType.Shallow).SingleOrDefault();

            return existing;
        }

        public static bool IsFirstUser(string userKey)
        {
            if (userKey == DebugUserNameAndKey || userKey == null) return false;
            var users = new UserContext().GetAll<User>(UserContext.ReadType.Shallow);
            var anyUser = users.Any(d => d.UserKey != DebugUserNameAndKey);
            return !anyUser;
        }

        public static User GetDebugUser()
        {
            var context = new UserContext();
            var users = context.Search<User>("UserKey", DebugUserNameAndKey, UserContext.ReadType.Shallow);
            var existing = users.FirstOrDefault();
            if (existing != null) return existing;

            var debugUser = new User(DebugUserNameAndKey, DebugUserNameAndKey, UserType.Admin);
            context.AddDefault(debugUser);
            return debugUser;
        }
    }
}

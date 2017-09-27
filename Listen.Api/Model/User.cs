using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Schema.Types;

namespace Listen.Api.Model
{
    public class User : NodeBase<User>
    {
        public string UserName { get; }
        public string UserKey { get; }
        public int UserType { get; }

        public User(string userName, string userKey, UserType userType)
        {
            UserName = userName;
            UserKey = userKey;
            UserType = (int)userType;
        }
    }

    public enum UserType
    {
        Normal = 0,
        Admin = 1,
        Pending = 2,
        Rejected = 3
    }
}

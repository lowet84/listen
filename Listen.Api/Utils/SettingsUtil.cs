using System.Linq;
using GraphQlRethinkDbLibrary;
using Listen.Api.Model;

namespace Listen.Api.Utils
{
    public static class SettingsUtil
    {
        public static Settings Settings => new UserContext().Search<Settings>("id", "", UserContext.ReadType.Shallow).FirstOrDefault();
    }
}

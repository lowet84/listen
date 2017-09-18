using System.Linq;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using Listen.Api.Model;

namespace Listen.Api.Schema
{
    public partial class Mutation
    {
        [Description("Save app settings")]
        public DefaultResult<Settings> SaveSettings(
            UserContext context,
            string path)
        {
            var oldSettings = context.Search<Settings>("Path", "", UserContext.ReadType.Shallow)?.FirstOrDefault();

            var newSettings = new Settings(
                path ?? oldSettings?.Path);

            if (newSettings.Equals(oldSettings))
            {
                return new DefaultResult<Settings>(newSettings);
            }

            if (oldSettings == null)
            {
                context.AddDefault(newSettings);
            }
            else
            {
                context.UpdateDefault(newSettings, oldSettings.Id);
            }

            return new DefaultResult<Settings>(newSettings);
        }
    }
}

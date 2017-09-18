using GraphQlRethinkDbLibrary.Schema.Types;
using GraphQL.Conventions;

namespace Listen.Api.Model
{
    public class Settings : NodeBase<Settings>
    {
        [Description("The base path where audio files are stored")]
        public string Path { get; }

        public Settings(string path)
        {
            Path = path;
        }

        public override bool Equals(object obj)
        {
            var settings = obj as Settings;
            return
                settings?.Path == Path;
        }
    }
}

using GraphQlRethinkDbLibrary.Schema.Types;
using GraphQL.Conventions;

namespace Listen.Api.Model
{
    public class Settings : NodeBase<Settings>
    {
        [Description("The base path where audio files are stored")]
        public string Path { get; }

        [Description("The minimum amount of user reviews needed to add a book automatically")]
        public int AutoMatchThreshold { get; }

        public Settings(string path, int? autoMatchThreshold)
        {
            Path = path;
            AutoMatchThreshold = autoMatchThreshold ?? 100;
        }

        public override bool Equals(object obj)
        {
            var settings = obj as Settings;
            return
                settings?.Path == Path &&
                settings.AutoMatchThreshold == AutoMatchThreshold;
        }
    }
}

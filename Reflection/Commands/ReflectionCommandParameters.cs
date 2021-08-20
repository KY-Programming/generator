using KY.Generator.Command;
using KY.Generator.Languages;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionCommandParameters : GeneratorCommandParameters
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SkipSelf { get; set; }
        public ILanguage Language { get; set; } = TypeScriptLanguage.Instance;
    }
}

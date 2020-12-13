using KY.Generator.Command;
using KY.Generator.Languages;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionCommandParameters : GeneratorCommandParameters
    {
        [GeneratorParameter("fromAttribute")]
        [GeneratorParameter("fromAttributes")]
        public bool FromAttributes { get; set; }

        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SkipSelf { get; set; }
        public ILanguage Language { get; set; } = TypeScriptLanguage.Instance;
        public string Using { get; set; }
    }
}
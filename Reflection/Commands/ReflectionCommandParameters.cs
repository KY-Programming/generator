using KY.Generator.Command;
using KY.Generator.Languages;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionCommandParameters : GeneratorCommandParameters
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool OnlySubTypes { get; set; }
        public ILanguage Language { get; set; }
    }
}

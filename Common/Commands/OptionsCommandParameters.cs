using KY.Generator.Command;

namespace KY.Generator.Commands
{
    internal class OptionsCommandParameters : GeneratorCommandParameters
    {
        public string Option { get; set; }
        public string Value { get; set; }
    }
}

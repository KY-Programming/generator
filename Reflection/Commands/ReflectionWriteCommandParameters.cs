using KY.Generator.Command;

namespace KY.Generator.Reflection.Commands
{
    internal class ReflectionWriteCommandParameters : GeneratorCommandParameters
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}
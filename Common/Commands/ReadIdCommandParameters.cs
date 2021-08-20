using KY.Generator.Command;

namespace KY.Generator.Commands
{
    internal class ReadIdCommandParameters : GeneratorCommandParameters
    {
        public string Project { get; set; }
        public string Solution { get; set; }
    }
}
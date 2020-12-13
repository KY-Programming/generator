using KY.Generator.Command;

namespace KY.Generator.AspDotNet.Commands
{
    internal class AspDotNetReadControllerCommandParameters : GeneratorCommandParameters
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
    }
}
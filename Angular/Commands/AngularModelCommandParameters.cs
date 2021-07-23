using KY.Generator.Command;

namespace KY.Generator.Angular.Commands
{
    public class AngularModelCommandParameters : GeneratorCommandParameters
    {
        public string Namespace { get; set; }
        public bool Strict { get; set; }
    }
}

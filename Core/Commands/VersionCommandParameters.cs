using KY.Generator.Command;

namespace KY.Generator.Commands
{
    internal class VersionCommandParameters : GeneratorCommandParameters
    {
        [GeneratorParameter("d")]
        public bool ShowDetailed { get; set; }
    }
}
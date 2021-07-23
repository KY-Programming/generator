using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Angular.Commands
{
    public class AngularServiceCommandParameters : GeneratorCommandParameters
    {
        public string Name { get; set; }
        public string RelativeModelPath { get; set; }
        public bool EndlessTries { get; set; }
        public List<int> Timeouts { get; set; }
        public bool Strict { get; set; }
    }
}

using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Angular.Commands
{
    public class AngularPackageCommandParameters : GeneratorCommandParameters
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public List<AngularPackageDependsOnParameter> DependsOn { get; set; } = new();
        public bool Build { get; set; }
        public bool Publish { get; set; }
        public bool PublishLocal { get; set; }
    }
}

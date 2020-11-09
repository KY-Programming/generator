using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public class GeneratorEnvironment
    {
        public List<ITransferObject> TransferObjects { get; }
        public bool SwitchContext { get; set; }
        public ProcessorArchitecture? SwitchToArchitecture { get; set; }
        public FrameworkName SwitchToFramework { get; set; }
        public bool IsBeforeBuild { get; set; }

        public GeneratorEnvironment()
        {
            this.TransferObjects = new List<ITransferObject>();
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using KY.Generator.Command;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public class GeneratorEnvironment
    {
        public List<ITransferObject> TransferObjects { get; }
        public bool IsBeforeBuild { get; set; }
        public bool IsMsBuild { get; set; }
        public string Command { get; set; }
        public List<CommandValueParameter> Parameters { get; set; }
        public bool SwitchContext { get; set; }
        public ProcessorArchitecture? SwitchToArchitecture { get; set; }
        public SwitchableFramework SwitchToFramework { get; set; }
        public bool SwitchToAsync { get; set; }
        public bool IsOnlyAsync { get; set; }

        public GeneratorEnvironment()
        {
            this.TransferObjects = new List<ITransferObject>();
        }
    }
}
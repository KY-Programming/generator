using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Command
{
    public class SwitchContextResult : IGeneratorCommandResult
    {
        public bool Success => false;
        public bool SwitchContext => true;
        public ProcessorArchitecture? SwitchToArchitecture { get; }
        public SwitchableFramework SwitchToFramework { get; }
        public bool SwitchToAsync => false;

        public SwitchContextResult(ProcessorArchitecture? switchToArchitecture, SwitchableFramework switchToFramework)
        {
            this.SwitchToArchitecture = switchToArchitecture;
            this.SwitchToFramework = switchToFramework;
        }
    }
}
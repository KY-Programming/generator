using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Command
{
    public class SuccessResult : IGeneratorCommandResult
    {
        public bool Success => true;
        public bool SwitchContext => false;
        public ProcessorArchitecture? SwitchToArchitecture => null;
        public SwitchableFramework SwitchToFramework => SwitchableFramework.None;
        public bool SwitchToAsync => false;
        public bool RerunOnAsync { get; private set; }

        public SuccessResult ForceRerunOnAsync()
        {
            this.RerunOnAsync = true;
            return this;
        }
    }
}
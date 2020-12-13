using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Command
{
    public class ErrorResult : IGeneratorCommandResult
    {
        public bool Success => false;
        public bool SwitchContext => false;
        public ProcessorArchitecture? SwitchToArchitecture => null;
        public SwitchableFramework SwitchToFramework => SwitchableFramework.None;
        public bool SwitchToAsync => false;
    }
}
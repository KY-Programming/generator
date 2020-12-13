using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Command
{
    public class SwitchAsyncResult : IGeneratorCommandResult
    {
        public bool Success => false;
        public bool SwitchContext => true;
        public ProcessorArchitecture? SwitchToArchitecture => null;
        public SwitchableFramework SwitchToFramework => SwitchableFramework.None;
        public bool SwitchToAsync => true;
    }
}
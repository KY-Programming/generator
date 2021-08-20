using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Command
{
    public interface IGeneratorCommandResult
    {
        bool Success { get; }
        bool SwitchContext { get; }
        ProcessorArchitecture? SwitchToArchitecture { get; }
        SwitchableFramework SwitchToFramework { get; }
        bool SwitchToAsync { get; }
        bool RerunOnAsync { get; }
    }
}
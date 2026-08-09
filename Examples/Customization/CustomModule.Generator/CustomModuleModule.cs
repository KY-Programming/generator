using CustomModule.Commands;
using CustomModule.Generator.Commands;
using KY.Core.Dependency;
using KY.Generator;
using KY.Generator.Csharp;
using KY.Generator.Models;

namespace CustomModule.Generator;

/// <summary>
/// The generator scans the assembly named in [assembly: GenerateWith] for GeneratorModule subclasses and
/// instantiates this one once per run. Registering the command is what makes it reachable - by name from
/// the command line, and through the parameters the fluent API produces.
/// </summary>
public class CustomModuleModule : GeneratorModule
{
    public CustomModuleModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        // Pulls in the C# language and its writers. DependsOn<TypeScriptModule>(), <AngularModule>() or
        // <ReflectionModule>() work the same way - that is how a module builds on the existing readers
        // and writers instead of reimplementing them.
        this.DependsOn<CsharpModule>();

        this.Register<WriteMessageCommand>(WriteMessageCommandParameters.Names);
    }
}

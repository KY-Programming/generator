using KY.Core.Dependency;
using KY.Generator;
using KY.Generator.Csharp;
using KY.Generator.Models;
using MyModule.Commands;
using MyModule.Generator.Commands;

namespace MyModule.Generator;

/// <summary>
/// The generator finds this class by scanning the assembly named in [assembly: GenerateWith] and
/// instantiates it once per run. Registering a command is what makes it reachable - by name from the
/// command line, and through the parameters the annotation and the fluent API produce.
/// </summary>
public class SampleModuleModule : GeneratorModule
{
    public SampleModuleModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        // Pulls in the C# language and writers. Use DependsOn<TypeScriptModule>(),
        // <AngularModule>() or <ReflectionModule>() to build on those instead - or on top.
        this.DependsOn<CsharpModule>();

        this.Register<SayHelloCommand>(SayHelloCommandParameters.Names);
    }
}

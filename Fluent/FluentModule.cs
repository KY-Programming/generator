using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Commands;

namespace KY.Generator;

public class FluentModule : ModuleBase
{
    public FluentModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<FluentCommand>(FluentCommand.Names);
    }
}
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator.Watchdog;

public class WatchdogModule : ModuleBase
{
    public WatchdogModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<WatchdogCommand>(WatchdogCommandParameters.Names);
    }
}

using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator.Watchdog
{
    public class WatchdogModule : ModuleBase
    {
        public WatchdogModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<WatchdogCommand>();
        }
    }
}
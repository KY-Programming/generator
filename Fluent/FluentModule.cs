using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Fluent;

namespace KY.Generator
{
    public class FluentModule : ModuleBase
    {
        public FluentModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<FluentCommand>();
        }
    }
}
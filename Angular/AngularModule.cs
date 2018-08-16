using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;

namespace KY.Generator.Angular
{
    public class AngularModule : ModuleBase
    {
        public AngularModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<ICommandGenerator>().To<GenerateAngularConfig>();
        }
    }
}
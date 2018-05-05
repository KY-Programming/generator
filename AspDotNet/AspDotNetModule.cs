using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Models;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetModule : ModuleBase
    {
        public AspDotNetModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGenerator>().To<GeneratorGenerator>();
            this.DependencyResolver.Bind<IConfigurationReader>().To<GeneratorConfigurationReader>();
        }
    }
}
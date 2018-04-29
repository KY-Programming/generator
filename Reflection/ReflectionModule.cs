using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Configuration;
using KY.Generator.Reflection.Extensions;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : ModuleBase
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGenerator>().To<ReflectionGenerator>();
            this.DependencyResolver.Bind<IConfigurationReader>().To<ReflectionConfigurationReader>();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}
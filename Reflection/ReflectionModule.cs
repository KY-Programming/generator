using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.Reflection.Configuration;
using KY.Generator.Reflection.Extensions;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : GeneratorModule
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<ReflectionGeneratorConfiguration>().ToSingleton();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
            StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
        }

        public override void BeforeConfigure()
        {
            ReflectionGeneratorConfiguration configuration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
            this.DependencyResolver.Bind<IGenerator>().To(configuration.Generator ?? (IGenerator)this.DependencyResolver.Create(configuration.GeneratorType));
            this.DependencyResolver.Bind<IConfigurationReader>().To(configuration.ConfigurationReader ?? (IConfigurationReader)this.DependencyResolver.Create(configuration.ConfigurationReaderType));
        }
    }
}
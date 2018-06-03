using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.OData.Configuration;
using KY.Generator.OData.Extensions;

namespace KY.Generator.OData
{
    public class ODataModule : GeneratorModule
    {
        public ODataModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<ODataGeneratorConfiguration>().ToSingleton();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
            StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<ODataGeneratorConfiguration>();
        }

        public override void BeforeConfigure()
        {
            ODataGeneratorConfiguration configuration = this.DependencyResolver.Get<ODataGeneratorConfiguration>();
            this.DependencyResolver.Bind<IGenerator>().To(configuration.Generator ?? (IGenerator)this.DependencyResolver.Create(configuration.GeneratorType));
            this.DependencyResolver.Bind<IConfigurationReader>().To(configuration.ConfigurationReader ?? (IConfigurationReader)this.DependencyResolver.Create(configuration.ConfigurationReaderType));
        }
    }
}
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.Tsql.Configuration;
using KY.Generator.Tsql.Extensions;

namespace KY.Generator.Tsql
{
    public class TsqlModule : GeneratorModule
    {
        public TsqlModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<TsqlGeneratorConfiguration>().ToSingleton();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
            StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<TsqlGeneratorConfiguration>();
        }

        public override void BeforeConfigure()
        {
            TsqlGeneratorConfiguration configuration = this.DependencyResolver.Get<TsqlGeneratorConfiguration>();
            this.DependencyResolver.Bind<IGenerator>().To(configuration.Generator ?? (IGenerator)this.DependencyResolver.Create(configuration.GeneratorType));
            this.DependencyResolver.Bind<IConfigurationReader>().To(configuration.ConfigurationReader ?? (IConfigurationReader)this.DependencyResolver.Create(configuration.ConfigurationReaderType));
        }
    }
}
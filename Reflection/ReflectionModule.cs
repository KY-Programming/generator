using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.Reflection.Commands;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : GeneratorModule
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<ReflectionModelReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionWriter>().ToSelf();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReflectionCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<RunByAttributeCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReflectionReadCommand>();
        }

        public override void Initialize()
        {
            //this.DependencyResolver.Bind<ReflectionGeneratorConfiguration>().ToSingleton();
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            //StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<ReflectionReadConfiguration, ReflectionReader>("reflection")
                .Map<ReflectionWriteConfiguration, ReflectionWriter>("reflection");
        }

        public override void BeforeConfigure()
        {
            //ReflectionGeneratorConfiguration configuration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
            //this.DependencyResolver.Bind<IGenerator>().To(configuration.Generator ?? (IGenerator)this.DependencyResolver.Create(configuration.GeneratorType));
            //this.DependencyResolver.Bind<IConfigurationReader>().To(configuration.ConfigurationReader ?? (IConfigurationReader)this.DependencyResolver.Create(configuration.ConfigurationReaderType));
        }
    }
}
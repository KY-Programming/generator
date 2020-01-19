using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Output;

namespace KY.Generator.Core.Tests
{
    public class TestConfigurationRunner
    {
        public IDependencyResolver Resolver { get; }
        public MemoryOutput Output { get; }

        public TestConfigurationRunner()
        {
            this.Resolver = new DependencyResolver();
            this.Resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.Resolver.Bind<CommandReader>().ToSelf();
            this.Resolver.Bind<CommandRegistry>().ToSingleton();
            this.Resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.Resolver.Bind<ConfigurationRunner>().ToSelf();

            this.Output = this.Resolver.Create<MemoryOutput>();
        }

        public TestConfigurationRunner Initialize<TModule>()
            where TModule : ModuleBase
        {
            this.Resolver.Create<TModule>().Initialize();
            return this;
        }

        public ExecuteConfiguration Parse(string json)
        {
            return this.Resolver.Create<ConfigurationsReader>().Parse(json);
        }

        public bool Run(string json)
        {
            ExecuteConfiguration configuration = this.Parse(json);
            configuration.Execute.Flatten().ForEach(x => x.Output = this.Output);
            ConfigurationRunner runner = this.Resolver.Create<ConfigurationRunner>();
            configuration.Execute.OfType<ConfigurationBase>().ForEach(x => x.AddHeader = false);
            return runner.Run(configuration.Execute);
        }
    }
}
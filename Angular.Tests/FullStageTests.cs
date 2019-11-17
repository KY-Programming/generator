using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Tests.Properties;
using KY.Generator.AspDotNet;
using KY.Generator.Configuration;
using KY.Generator.Csharp;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Reflection;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Angular.Tests
{
    [TestClass]
    public class FullStageTests
    {
        private IDependencyResolver resolver;
        private ConfigurationsReader reader;
        private ConfigurationRunner runner;
        private MemoryOutput output;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ReaderConfigurationMapping>().ToSingleton();
            this.resolver.Bind<WriterConfigurationMapping>().ToSingleton();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<CsharpModule>().Initialize();
            this.resolver.Create<AspDotNetModule>().Initialize();
            this.resolver.Create<AngularModule>().Initialize();
            this.resolver.Create<ReflectionModule>().Initialize();
            this.resolver.Create<TypeScriptModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
            this.runner = this.resolver.Create<ConfigurationRunner>();
            this.output = this.resolver.Create<MemoryOutput>();
        }

        [TestMethod]
        public void CustomHttpClientType()
        {
            Assert.AreEqual(true, this.Run(Resources.custom_client_type), "Generation not successful");
            Assert.AreEqual(2, this.output.Files.Count);
            Assert.AreEqual(Resources.custom_values_service_ts, this.output.Files["src\\app\\services\\custom-values-service.ts"]);
        }

        private bool Run(string configuration)
        {
            List<ConfigurationSet> configurations = this.reader.Parse(configuration);
            configurations.ForEach(x => x.Writers.ForEach(y => y.AddHeader = false));
            return this.runner.Run(configurations, this.output);
        }
    }
}

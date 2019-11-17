using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet;
using KY.Generator.Configuration;
using KY.Generator.Csharp;
using KY.Generator.EntityFramework;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Tests.Properties;
using KY.Generator.Tsql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
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
            this.resolver.Create<TsqlModule>().Initialize();
            this.resolver.Create<AspDotNetModule>().Initialize();
            this.resolver.Create<EntityFrameworkModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
            this.runner = this.resolver.Create<ConfigurationRunner>();
            this.output = this.resolver.Create<MemoryOutput>();
        }

        [TestMethod]
        public void FullProject()
        {
            Assert.AreEqual(true, this.Run(Resources.full_project_generator), "Generation not successful");
            Assert.AreEqual(4, this.output.Files.Count);
            Assert.AreEqual(Resources.User_cs, this.output.Files["Models\\User.cs"]);
            Assert.AreEqual(Resources.UserController_cs, this.output.Files["Controllers\\UserController.cs"]);
            Assert.AreEqual(Resources.UserRepository_cs, this.output.Files["Repositories\\UserRepository.cs"]);
            Assert.AreEqual(Resources.DataContext, this.output.Files["Repositories\\DataContext.cs"]);
        }

        private bool Run(string configuration)
        {
            List<ConfigurationSet> configurations = this.reader.Parse(configuration);
            configurations.ForEach(x => x.Writers.ForEach(y => y.AddHeader = false));
            return this.runner.Run(configurations, this.output);
        }
    }
}

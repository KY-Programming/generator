using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Angular;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.OData.Tests.Properties;
using KY.Generator.Output;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.OData.Tests
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
            this.resolver.Bind<ConfigurationMapping>().ToSingleton();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<ODataModule>().Initialize();
            this.resolver.Create<TypeScriptModule>().Initialize();
            this.resolver.Create<AngularModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
            this.runner = this.resolver.Create<ConfigurationRunner>();
            this.output = this.resolver.Create<MemoryOutput>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(true, this.Run(Resources.odata_generator), "Generation not successful");
            Assert.AreEqual(4, this.output.Files.Count);
            Assert.AreEqual(Resources.address_service_ts, this.output.Files["address.service.ts"]);
            Assert.AreEqual(Resources.address_ts, this.output.Files["address.ts"]);
            Assert.AreEqual(Resources.user_ts, this.output.Files["user.ts"]);
            Assert.AreEqual(Resources.u_se_r_service_ts, this.output.Files["u-se-r.service.ts"]);
        }

        private bool Run(string configuration)
        {
            List<ConfigurationSet> sets = this.reader.Parse(configuration);
            sets.ForEach(x => x.Configurations.ForEach(y => y.AddHeader = false));
            sets.SelectMany(x => x.Configurations).OfType<AngularWriteConfiguration>().ForEach(x => x.Model.AddHeader = false);
            return this.runner.Run(sets, this.output);
        }
    }
}

using KY.Core;
using KY.Generator.Angular.Tests.Properties;
using KY.Generator.AspDotNet;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using KY.Generator.Reflection;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Angular.Tests
{
    [TestClass]
    public class FullStageTests : MsTestBase
    {
        private TestConfigurationRunner runner;

        [TestInitialize]
        public void Initialize()
        {
            this.runner = new TestConfigurationRunner();
            this.runner.Resolver.Create<CoreModule>().Initialize();
            this.runner.Resolver.Create<CsharpModule>().Initialize();
            this.runner.Resolver.Create<AspDotNetModule>().Initialize();
            this.runner.Resolver.Create<AngularModule>().Initialize();
            this.runner.Resolver.Create<ReflectionModule>().Initialize();
            this.runner.Resolver.Create<TypeScriptModule>().Initialize();
        }

        [TestMethod]
        public void CustomHttpClientType()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.custom_client_type), "Generation not successful");
            Assert.AreEqual(2, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.custom_values_service_ts, this.runner.Output.Files["src\\app\\services\\custom-values.service.ts"]);
        }
    }
}
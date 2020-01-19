using KY.Core;
using KY.Generator.AspDotNet.Tests.Properties;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.AspDotNet.Tests
{
    [TestClass]
    public class FullStageTests : MsTestBase
    {
        private TestConfigurationRunner runner;

        [TestInitialize]
        public void Initialize()
        {
            this.runner = new TestConfigurationRunner()
                          .Initialize<CoreModule>()
                          .Initialize<CsharpModule>()
                          .Initialize<AspDotNetModule>();
        }

        [TestMethod]
        public void FrameworkController()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.generator_controller_generator), "Generation not successful");
            Assert.AreEqual(1, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.GeneratorController_cs, this.runner.Output.Files["GeneratorController.cs"]);
        }

        [TestMethod]
        public void CoreController()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.generator_controller_core_generator), "Generation not successful");
            Assert.AreEqual(1, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.GeneratorCoreController_cs, this.runner.Output.Files["GeneratorController.cs"]);
        }
    }
}
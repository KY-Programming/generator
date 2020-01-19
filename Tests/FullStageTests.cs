using KY.Core;
using KY.Generator.AspDotNet;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using KY.Generator.EntityFramework;
using KY.Generator.Tests.Properties;
using KY.Generator.Tsql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
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
                          .Initialize<TsqlModule>()
                          .Initialize<AspDotNetModule>()
                          .Initialize<EntityFrameworkModule>();
        }

        [TestMethod]
        public void FullProject()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.full_project_generator), "Generation not successful");
            Assert.AreEqual(4, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.User_cs, this.runner.Output.Files["Models\\User.cs"]);
            Assert.AreEqual(Resources.UserController_cs, this.runner.Output.Files["Controllers\\UserController.cs"]);
            Assert.AreEqual(Resources.UserRepository_cs, this.runner.Output.Files["Repositories\\UserRepository.cs"]);
            Assert.AreEqual(Resources.DataContext, this.runner.Output.Files["Repositories\\DataContext.cs"]);
        }
    }
}
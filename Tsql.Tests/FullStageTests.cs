using KY.Core;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using KY.Generator.Tsql.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tsql.Tests
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
                          .Initialize<TsqlModule>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.tsql_generator), "Generation not successful");
            Assert.AreEqual(1, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.User_cs, this.runner.Output.Files["User.cs"]);
        }
    }
}
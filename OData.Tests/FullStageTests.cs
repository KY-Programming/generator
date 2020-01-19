using KY.Core;
using KY.Generator.Angular;
using KY.Generator.Core.Tests;
using KY.Generator.OData.Tests.Properties;
using KY.Generator.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.OData.Tests
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
                          .Initialize<ODataModule>()
                          .Initialize<TypeScriptModule>()
                          .Initialize<AngularModule>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.odata_generator), "Generation not successful");
            Assert.AreEqual(4, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.address_service_ts, this.runner.Output.Files["address.service.ts"]);
            Assert.AreEqual(Resources.address_ts, this.runner.Output.Files["address.ts"]);
            Assert.AreEqual(Resources.user_ts, this.runner.Output.Files["user.ts"]);
            Assert.AreEqual(Resources.u_se_r_service_ts, this.runner.Output.Files["u-se-r.service.ts"]);
        }
    }
}
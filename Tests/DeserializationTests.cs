using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Core.Tests;
using KY.Generator.Exceptions;
using KY.Generator.Tests.Models;
using KY.Generator.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    [TestClass]
    public class DeserializationTests : MsTestBase
    {
        private TestConfigurationRunner runner;

        [TestInitialize]
        public void Initialize()
        {
            this.runner = new TestConfigurationRunner()
                .Initialize<CoreModule>();
            this.runner.Resolver.Get<CommandRegistry>()
                .Register<Read1Command, Read1Configuration>("1", "read")
                .Register<Read2Command, Read2Configuration>("2", "read")
                .Register<Write1Command, Write1Configuration>("1", "write")
                .Register<Write2Command, Write2Configuration>("2", "write");
        }

        [TestMethod]
        public void EmptyConfiguration()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(Resources.empty_configuration));
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownCommandException))]
        public void EmptyGenerate()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            reader.Parse(Resources.empty_generate);
        }

        [TestMethod]
        public void OneGenerate()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            ExecuteConfiguration configuration = reader.Parse(Resources.one_generate);
            Assert.AreEqual(1, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", configuration.Execute[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneGenerateInArray()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            ExecuteConfiguration configuration = reader.Parse(Resources.one_generate_in_array);
            Assert.AreEqual(1, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", configuration.Execute[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidConfigurationException))]
        public void OneGenerateInArrayArray()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            ExecuteConfiguration configuration = reader.Parse(Resources.one_generate_in_array_array);
        }

        [TestMethod]
        public void OneReadOneGenerate()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            ExecuteConfiguration configuration = reader.Parse(Resources.one_read_one_generate);
            Assert.AreEqual(2, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", configuration.Execute[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configuration.Execute[1].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void TwoReadsOneGenerate()
        {
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            ExecuteConfiguration configuration = reader.Parse(Resources.two_reads_one_generate);
            Assert.AreEqual(3, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", configuration.Execute[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configuration.Execute[1].CastTo<Read2Configuration>().Property2);
            Assert.AreEqual("Test3", configuration.Execute[2].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OldConfig()
        {
            string json = Resources.old_config;
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(json));
        }

        [TestMethod]
        public void OldConfigXml()
        {
            string json = Resources.old_config1;
            ConfigurationsReader reader = this.runner.Resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(json));
        }
    }
}
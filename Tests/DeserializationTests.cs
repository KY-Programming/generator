using System.Collections.Generic;
using System.Linq;
using System.Text;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Tests.Models;
using KY.Generator.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests
{
    [TestClass]
    public class DeserializationTests
    {
        private IDependencyResolver resolver;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ConfigurationMapping>().ToSingleton();
            this.resolver.Get<ConfigurationMapping>()
                .Map<Read1Configuration, Reader1>("1")
                .Map<Read2Configuration, Reader2>("2")
                .Map<Write1Configuration, Writer1>("1")
                .Map<Write2Configuration, Writer2>("2");
            this.resolver.Create<CoreModule>().Initialize();
        }

        [TestMethod]
        public void EmptyConfiguration()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(Resources.empty_configuration));
        }

        [TestMethod]
        public void EmptyGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.empty_generate);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(0, sets[0].Configurations.Count, "Unexpected number of configurations");
        }

        [TestMethod]
        public void EmptyConfigurationArray()
        {
            //string json = Encoding.UTF8.GetString(Resources.empty_configuration_array);
            //ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            //List<ConfigurationSet> configurations = reader.Parse(json);
            //Assert.AreEqual(2, configurations.Count, "Unexpected number of configurations");
            Assert.Inconclusive("Not implemented yet");
        }

        [TestMethod]
        public void OneGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.one_generate);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(1, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneGenerateInArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.one_generate_in_array);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(1, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneGenerateInArrayArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.one_generate_in_array_array);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(1, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void TwoGenerateInArrayArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.two_generates_in_array_array);
            Assert.AreEqual(2, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(1, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Write1Configuration>().Property1);
            Assert.AreEqual(1, sets[1].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test2", sets[1].Configurations[0].CastTo<Write2Configuration>().Property2);
        }

        [TestMethod]
        public void OneReadOneGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.one_read_one_generate);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(2, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", sets[0].Configurations[1].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void TwoReadsOneGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> sets = reader.Parse(Resources.two_reads_one_generate);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(3, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Test1", sets[0].Configurations[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", sets[0].Configurations[1].CastTo<Read2Configuration>().Property2);
            Assert.AreEqual("Test3", sets[0].Configurations[2].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneReadOneGenerateTwice()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_read_one_generate_twice);
            Assert.AreEqual(2, configurations.Count, "Unexpected number of sets");
            Assert.AreEqual(2, configurations[0].Configurations.Count, "Unexpected number of config 1 configurations");
            Assert.AreEqual(2, configurations[1].Configurations.Count, "Unexpected number of config 2 configurations");
            Assert.AreEqual("Test1", configurations[0].Configurations[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configurations[0].Configurations[1].CastTo<Write1Configuration>().Property1);
            Assert.AreEqual("Test3", configurations[1].Configurations[0].CastTo<Read2Configuration>().Property2);
            Assert.AreEqual("Test4", configurations[1].Configurations[1].CastTo<Write2Configuration>().Property2);
        }

        [TestMethod]
        public void OldConfig()
        {
            string json = Resources.old_config;
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(json));
        }

        [TestMethod]
        public void OldConfigXml()
        {
            string json = Resources.old_config1;
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            Assert.ThrowsException<InvalidConfigurationException>(() => reader.Parse(json));
        }
    }
}
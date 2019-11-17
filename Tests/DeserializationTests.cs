using System.Collections.Generic;
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
            this.resolver.Bind<ReaderConfigurationMapping>().ToSingleton();
            this.resolver.Get<ReaderConfigurationMapping>()
                .Map<Read1Configuration, Reader1>("1")
                .Map<Read2Configuration, Reader2>("2");
            this.resolver.Bind<WriterConfigurationMapping>().ToSingleton();
            this.resolver.Get<WriterConfigurationMapping>()
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
            List<ConfigurationSet> configurations = reader.Parse(Resources.empty_generate);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(0, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(0, configurations[0].Writers.Count, "Unexpected number of writers");
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
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_generate);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(0, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneGenerateInArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_generate_in_array);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(0, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneGenerateInArrayArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_generate_in_array_array);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(0, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void TwoGenerateInArrayArray()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.two_generates_in_array_array);
            Assert.AreEqual(2, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(0, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
            Assert.AreEqual(0, configurations[1].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[1].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test2", configurations[1].Writers[0].CastTo<Write2Configuration>().Property2);
        }

        [TestMethod]
        public void OneReadOneGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_read_one_generate);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Readers[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void TwoReadsOneGenerate()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.two_reads_one_generate);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(2, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            Assert.AreEqual("Test1", configurations[0].Readers[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configurations[0].Readers[1].CastTo<Read2Configuration>().Property2);
            Assert.AreEqual("Test3", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
        }

        [TestMethod]
        public void OneReadOneGenerateTwice()
        {
            ConfigurationsReader reader = this.resolver.Create<ConfigurationsReader>();
            List<ConfigurationSet> configurations = reader.Parse(Resources.one_read_one_generate_twice);
            Assert.AreEqual(2, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of config 1 readers");
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of config 1 writers");
            Assert.AreEqual(1, configurations[1].Readers.Count, "Unexpected number of config 2 readers");
            Assert.AreEqual(1, configurations[1].Writers.Count, "Unexpected number of config 2 writers");
            Assert.AreEqual("Test1", configurations[0].Readers[0].CastTo<Read1Configuration>().Property1);
            Assert.AreEqual("Test2", configurations[0].Writers[0].CastTo<Write1Configuration>().Property1);
            Assert.AreEqual("Test3", configurations[1].Readers[0].CastTo<Read2Configuration>().Property2);
            Assert.AreEqual("Test4", configurations[1].Writers[0].CastTo<Write2Configuration>().Property2);
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
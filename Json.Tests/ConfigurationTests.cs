using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Configuration;
using KY.Generator.Json.Tests.Properties;
using KY.Generator.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Json.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        private IDependencyResolver resolver;
        private ConfigurationsReader reader;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().To<TypeMapping>();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ReaderConfigurationMapping>().ToSingleton();
            this.resolver.Get<ReaderConfigurationMapping>();
            this.resolver.Bind<WriterConfigurationMapping>().ToSingleton();
            this.resolver.Get<WriterConfigurationMapping>();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<CsharpModule>().Initialize();
            this.resolver.Create<JsonModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
        }

        [TestMethod]
        public void ReadSimpleConfiguration()
        {
            List<ConfigurationPair> configurations = this.reader.Parse(Resources.simple_generator);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual("Resources/simple.json", configurations[0].Readers[0].CastTo<JsonReadConfiguration>().Source);
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            JsonWriteConfiguration writeConfiguration = configurations[0].Writers[0].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithoutReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
        }

        [TestMethod]
        public void ReadSimpleWithReaderConfiguration()
        {
            List<ConfigurationPair> configurations = this.reader.Parse(Resources.simple_with_reader_generator);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual("Resources/simple.json", configurations[0].Readers[0].CastTo<JsonReadConfiguration>().Source);
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            JsonWriteConfiguration writeConfiguration = configurations[0].Writers[0].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
            Assert.AreEqual(true, writeConfiguration.Object.WithReader);

        }

        [TestMethod]
        public void ReadSimpleWithSeparateReaderConfiguration()
        {
            List<ConfigurationPair> configurations = this.reader.Parse(Resources.simple_with_separate_reader_generator);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual("Resources/simple.json", configurations[0].Readers[0].CastTo<JsonReadConfiguration>().Source);
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            JsonWriteConfiguration writeConfiguration = configurations[0].Writers[0].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("Simple", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
            Assert.AreEqual(false, writeConfiguration.Object.WithReader);
            Assert.IsNotNull(writeConfiguration.Reader);
        }

        [TestMethod]
        public void ReadComplexConfiguration()
        {
            List<ConfigurationPair> configurations = this.reader.Parse(Resources.complex_generator);
            Assert.AreEqual(1, configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual(1, configurations[0].Readers.Count, "Unexpected number of readers");
            Assert.AreEqual("Resources/complex.json", configurations[0].Readers[0].CastTo<JsonReadConfiguration>().Source);
            Assert.AreEqual(1, configurations[0].Writers.Count, "Unexpected number of writers");
            ModelWriteConfiguration writeConfiguration = configurations[0].Writers[0].CastTo<ModelWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("Complex", writeConfiguration.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Namespace);
            Assert.AreEqual(true, writeConfiguration.FormatNames);
        }
    }
}
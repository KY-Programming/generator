using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Configurations;
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
            this.resolver.Bind<ConfigurationMapping>().ToSingleton();
            this.resolver.Get<ConfigurationMapping>();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<CsharpModule>().Initialize();
            this.resolver.Create<JsonModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
        }

        [TestMethod]
        public void ReadSimpleConfiguration()
        {
            List<ConfigurationSet> sets = this.reader.Parse(Resources.simple_generator);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(2, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", sets[0].Configurations[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = sets[0].Configurations[1].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithoutReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
        }

        [TestMethod]
        public void ReadSimpleWithReaderConfiguration()
        {
            List<ConfigurationSet> sets = this.reader.Parse(Resources.simple_with_reader_generator);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(2, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", sets[0].Configurations[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = sets[0].Configurations[1].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
            Assert.AreEqual(true, writeConfiguration.Object.WithReader);

        }

        [TestMethod]
        public void ReadSimpleWithSeparateReaderConfiguration()
        {
            List<ConfigurationSet> sets = this.reader.Parse(Resources.simple_with_separate_reader_generator);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(2, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", sets[0].Configurations[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = sets[0].Configurations[1].CastTo<JsonWriteConfiguration>();
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
            List<ConfigurationSet> sets = this.reader.Parse(Resources.complex_generator);
            Assert.AreEqual(1, sets.Count, "Unexpected number of sets");
            Assert.AreEqual(2, sets[0].Configurations.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/complex.json", sets[0].Configurations[0].CastTo<JsonReadConfiguration>().Source);
            ModelWriteConfiguration writeConfiguration = sets[0].Configurations[1].CastTo<ModelWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("Complex", writeConfiguration.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Namespace);
            Assert.AreEqual(true, writeConfiguration.FormatNames);
        }
    }
}
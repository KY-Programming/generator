using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Json.Tests
{
    [TestClass]
    public class ConfigurationTests : MsTestBase
    {
        private TestConfigurationRunner runner;

        [TestInitialize]
        public void Initialize()
        {
            this.runner = new TestConfigurationRunner()
                          .Initialize<CoreModule>()
                          .Initialize<CsharpModule>()
                          .Initialize<JsonModule>();
        }

        [TestMethod]
        public void ReadSimpleConfiguration()
        {
            ExecuteConfiguration configuration = this.runner.Parse(Resources.simple_generator);
            Assert.AreEqual(2, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", configuration.Execute[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = configuration.Execute[1].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithoutReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
        }

        [TestMethod]
        public void ReadSimpleWithReaderConfiguration()
        {
            ExecuteConfiguration configuration = this.runner.Parse(Resources.simple_with_reader_generator);
            Assert.AreEqual(2, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", configuration.Execute[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = configuration.Execute[1].CastTo<JsonWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("SimpleWithReader", writeConfiguration.Object.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Object.Namespace);
            Assert.AreEqual(true, writeConfiguration.Object.FormatNames);
            Assert.AreEqual(true, writeConfiguration.Object.WithReader);

        }

        [TestMethod]
        public void ReadSimpleWithSeparateReaderConfiguration()
        {
            ExecuteConfiguration configuration = this.runner.Parse(Resources.simple_with_separate_reader_generator);
            Assert.AreEqual(2, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/simple.json", configuration.Execute[0].CastTo<JsonReadConfiguration>().Source);
            JsonWriteConfiguration writeConfiguration = configuration.Execute[1].CastTo<JsonWriteConfiguration>();
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
            ExecuteConfiguration configuration = this.runner.Parse(Resources.complex_generator);
            Assert.AreEqual(2, configuration.Execute.Count, "Unexpected number of configurations");
            Assert.AreEqual("Resources/complex.json", configuration.Execute[0].CastTo<JsonReadConfiguration>().Source);
            ModelWriteConfiguration writeConfiguration = configuration.Execute[1].CastTo<ModelWriteConfiguration>();
            Assert.AreEqual(CsharpLanguage.Instance, writeConfiguration.Language);
            Assert.AreEqual("Complex", writeConfiguration.Name);
            Assert.AreEqual("KY.Generator.Examples.Json", writeConfiguration.Namespace);
            Assert.AreEqual(true, writeConfiguration.FormatNames);
        }
    }
}
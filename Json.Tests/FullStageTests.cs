using KY.Core;
using KY.Generator.Core.Tests;
using KY.Generator.Csharp;
using KY.Generator.Json.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Json.Tests
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
                          .Initialize<JsonModule>();
        }

        [TestMethod]
        public void Simple()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.simple_generator), "Generation not successful");
            Assert.AreEqual(1, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.SimpleWithoutReader_cs, this.runner.Output.Files["SimpleWithoutReader.cs"]);
        }

        [TestMethod]
        public void SimpleWithReader()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.simple_with_reader_generator), "Generation not successful");
            Assert.AreEqual(1, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.SimpleWithReader_cs, this.runner.Output.Files["SimpleWithReader.cs"]);
        }

        [TestMethod]
        public void SimpleWithSeparateReader()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.simple_with_separate_reader_generator), "Generation not successful");
            Assert.AreEqual(2, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.Simple_cs, this.runner.Output.Files["Simple.cs"]);
            Assert.AreEqual(Resources.SimpleReader_cs, this.runner.Output.Files["SimpleReader.cs"]);
        }

        [TestMethod]
        public void Complex()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.complex_generator), "Generation not successful");
            Assert.AreEqual(3, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.Complex_cs, this.runner.Output.Files["Complex.cs"]);
            Assert.AreEqual(Resources.ObjectProperty_cs, this.runner.Output.Files["ObjectProperty.cs"]);
            Assert.AreEqual(Resources.ObjectArrayProperty_cs, this.runner.Output.Files["ObjectArrayProperty.cs"]);
        }

        [TestMethod]
        public void ComplexWithReader()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.complex_with_reader_generator), "Generation not successful");
            Assert.AreEqual(3, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.ComplexWithReader_cs, this.runner.Output.Files["ComplexWithReader.cs"]);
            Assert.AreEqual(Resources.ObjectProperty_cs, this.runner.Output.Files["ObjectProperty.cs"]);
            Assert.AreEqual(Resources.ObjectArrayProperty_cs, this.runner.Output.Files["ObjectArrayProperty.cs"]);
        }

        [TestMethod]
        public void FormatNames()
        {
            Assert.AreEqual(true, this.runner.Run(Resources.formatNames_generator), "Generation not successful");
            Assert.AreEqual(3, this.runner.Output.Files.Count);
            Assert.AreEqual(Resources.FormatNames_cs, this.runner.Output.Files["FormatNames.cs"]);
            Assert.AreEqual(Resources.Alllowerobject_cs, this.runner.Output.Files["Alllowerobject.cs"]);
            Assert.AreEqual(Resources.Allupperobject_cs, this.runner.Output.Files["Allupperobject.cs"]);
        }
    }
}
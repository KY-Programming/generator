using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Csharp;
using KY.Generator.Json.Tests.Properties;
using KY.Generator.Mappings;
using KY.Generator.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Json.Tests
{
    [TestClass]
    public class FullStageTests
    {
        private IDependencyResolver resolver;
        private ConfigurationsReader reader;
        private ConfigurationRunner runner;
        private MemoryOutput output;

        [TestInitialize]
        public void Initialize()
        {
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ReaderConfigurationMapping>().ToSingleton();
            this.resolver.Bind<WriterConfigurationMapping>().ToSingleton();
            this.resolver.Create<CoreModule>().Initialize();
            this.resolver.Create<CsharpModule>().Initialize();
            this.resolver.Create<JsonModule>().Initialize();
            this.reader = this.resolver.Create<ConfigurationsReader>();
            this.runner = this.resolver.Create<ConfigurationRunner>();
            this.output = this.resolver.Create<MemoryOutput>();
        }

        [TestMethod]
        public void Simple()
        {
            Assert.AreEqual(true, this.Run(Resources.simple_generator), "Generation not successful");
            Assert.AreEqual(1, this.output.Files.Count);
            Assert.AreEqual(Resources.SimpleWithoutReader_cs, this.output.Files["SimpleWithoutReader.cs"]);
        }

        [TestMethod]
        public void SimpleWithReader()
        {
            Assert.AreEqual(true, this.Run(Resources.simple_with_reader_generator), "Generation not successful");
            Assert.AreEqual(1, this.output.Files.Count);
            Assert.AreEqual(Resources.SimpleWithReader_cs, this.output.Files["SimpleWithReader.cs"]);
        }

        [TestMethod]
        public void SimpleWithSeparateReader()
        {
            Assert.AreEqual(true, this.Run(Resources.simple_with_separate_reader_generator), "Generation not successful");
            Assert.AreEqual(2, this.output.Files.Count);
            Assert.AreEqual(Resources.Simple_cs, this.output.Files["Simple.cs"]);
            Assert.AreEqual(Resources.SimpleReader_cs, this.output.Files["SimpleReader.cs"]);
        }

        [TestMethod]
        public void Complex()
        {
            Assert.AreEqual(true, this.Run(Resources.complex_generator), "Generation not successful");
            Assert.AreEqual(3, this.output.Files.Count);
            Assert.AreEqual(Resources.Complex_cs, this.output.Files["Complex.cs"]);
            Assert.AreEqual(Resources.ObjectProperty_cs, this.output.Files["ObjectProperty.cs"]);
            Assert.AreEqual(Resources.ObjectArrayProperty_cs, this.output.Files["ObjectArrayProperty.cs"]);
        }

        [TestMethod]
        public void ComplexWithReader()
        {
            Assert.AreEqual(true, this.Run(Resources.complex_with_reader_generator), "Generation not successful");
            Assert.AreEqual(3, this.output.Files.Count);
            Assert.AreEqual(Resources.ComplexWithReader_cs, this.output.Files["ComplexWithReader.cs"]);
            Assert.AreEqual(Resources.ObjectProperty_cs, this.output.Files["ObjectProperty.cs"]);
            Assert.AreEqual(Resources.ObjectArrayProperty_cs, this.output.Files["ObjectArrayProperty.cs"]);
        }

        [TestMethod]
        public void FormatNames()
        {
            Assert.AreEqual(true, this.Run(Resources.formatNames_generator), "Generation not successful");
            Assert.AreEqual(3, this.output.Files.Count);
            Assert.AreEqual(Resources.FormatNames_cs, this.output.Files["FormatNames.cs"]);
            Assert.AreEqual(Resources.Alllowerobject_cs, this.output.Files["Alllowerobject.cs"]);
            Assert.AreEqual(Resources.Allupperobject_cs, this.output.Files["Allupperobject.cs"]);
        }

        private bool Run(string configuration)
        {
            List<ConfigurationPair> configurations = this.reader.Parse(configuration);
            configurations.ForEach(x => x.Writers.ForEach(y => y.AddHeader = false));
            return this.runner.Run(configurations, this.output);
        }
    }
}

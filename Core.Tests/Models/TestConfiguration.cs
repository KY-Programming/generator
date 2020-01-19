using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Output;

namespace KY.Generator.Core.Tests.Models
{
    internal class TestConfiguration : IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool AddHeader { get; set; }
        public bool SkipNamespace { get; set; }
        public List<string> Usings { get; }
        public bool FormatNames { get; set; }
        public ILanguage Language { get; set; }
        public string LanguageKey { get; set; }
        public ConfigurationFormatting Formatting { get; }
        public IOutput Output { get; set; }
        public ConfigurationEnvironment Environment { get; set; }
        public bool BeforeBuild { get; set; }

        public TestConfiguration()
        {
            this.Language = new TestLanguage();
            this.Usings = new List<string>();
            this.Formatting = new ConfigurationFormatting();
            this.Environment = new ConfigurationEnvironment();
        }
    }

    internal class TestConfiguration2 : TestConfiguration
    { }

    internal class TestConfiguration3 : TestConfiguration
    { }

    internal class TestConfiguration4 : TestConfiguration
    { }
}
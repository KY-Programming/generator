using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
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
        public ConfigurationFormatting Formatting { get; }
        public IOutput Output { get; set; }
        public ConfigurationEnvironment Environment { get; set; }

        public TestConfiguration()
        {
            this.Language = new TestLanguage();
            this.Usings = new List<string>();
            this.Formatting = new ConfigurationFormatting();
        }
    }
}
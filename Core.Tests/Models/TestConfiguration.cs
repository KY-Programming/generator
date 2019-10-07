using KY.Generator.Configurations;
using KY.Generator.Languages;

namespace KY.Generator.Core.Tests.Models
{
    internal class TestConfiguration : IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool AddHeader { get; set; }
        public bool SkipNamespace { get; set; }
        public bool FieldsToProperties { get; set; }
        public bool PropertiesToFields { get; set; }
        public bool FormatNames { get; set; }
        public ILanguage Language { get; set; }

        public TestConfiguration()
        {
            this.Language = new TestLanguage();
        }
    }
}
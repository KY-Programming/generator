using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using Newtonsoft.Json;

namespace KY.Generator.Configurations
{
    public abstract class ConfigurationBase : IConfiguration
    {
        public bool VerifySsl { get; set; } = true;

        [JsonIgnore]
        public ILanguage Language { get; set; }

        [JsonProperty("Language")]
        internal string LanguageKey { get; set; }

        public bool AddHeader { get; set; } = true;
        public virtual bool RequireLanguage => true;

        public List<ClassMapping> ClassMapping { get; }
        public List<FieldMapping> FieldMapping { get; }
        public List<PropertyMapping> PropertyMapping { get; }
        public ConfigurationEnvironment Environment { get; set; }
        public ConfigurationFormatting Formatting { get; set; }
        public bool CheckOnOverwrite { get; set; } = true;
        public bool BeforeBuild { get; set; }

        protected ConfigurationBase()
        {
            this.ClassMapping = new List<ClassMapping>();
            this.FieldMapping = new List<FieldMapping>();
            this.PropertyMapping = new List<PropertyMapping>();
            this.Environment = new ConfigurationEnvironment(null, null);
            this.Formatting = new ConfigurationFormatting();
        }

        public virtual void CopyBaseFrom(ConfigurationBase source)
        {
            this.VerifySsl = source.VerifySsl;
            this.Language = source.Language;
            this.AddHeader = source.AddHeader;
            this.ClassMapping.AddRange(source.ClassMapping);
            this.FieldMapping.AddRange(source.FieldMapping);
            this.PropertyMapping.AddRange(source.PropertyMapping);
            this.Environment = source.Environment;
            this.Formatting = source.Formatting;
        }
    }
}
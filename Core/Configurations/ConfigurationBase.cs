using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Output;
using Newtonsoft.Json;

namespace KY.Generator.Configurations
{
    public abstract class ConfigurationBase : IConfiguration
    {
        public bool VerifySsl { get; set; } = true;

        [JsonIgnore]
        [ConfigurationIgnore]
        public ILanguage Language { get; set; }

        [JsonProperty("Language")]
        [ConfigurationProperty("Language")]
        internal string LanguageKey { get; set; }

        public bool AddHeader { get; set; } = true;
        public virtual bool RequireLanguage => true;

        [JsonIgnore]
        public bool SkipHeader
        {
            get => !this.AddHeader;
            set => this.AddHeader = !value;
        }

        public List<ClassMapping> ClassMapping { get; }
        public List<FieldMapping> FieldMapping { get; }
        public List<PropertyMapping> PropertyMapping { get; }
        public ConfigurationFormatting Formatting { get; set; }

        [JsonIgnore]
        public bool Standalone { get; set; }

        [JsonIgnore]
        public ConfigurationEnvironment Environment { get; set; }

        [JsonIgnore]
        public IOutput Output { get; }

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
            this.Standalone = source.Standalone;
            this.Environment = source.Environment;
            this.Formatting = source.Formatting;
        }
    }
}
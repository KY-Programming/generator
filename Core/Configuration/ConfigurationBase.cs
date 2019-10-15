using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using Newtonsoft.Json;

namespace KY.Generator.Configuration
{
    public abstract class ConfigurationBase
    {
        public string Framework { get; set; }
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
        public bool Standalone { get; set; }
        public string ConfigurationFilePath { get; set; }

        protected ConfigurationBase()
        {
            this.ClassMapping = new List<ClassMapping>();
            this.FieldMapping = new List<FieldMapping>();
            this.PropertyMapping = new List<PropertyMapping>();
        }

        public virtual void CopyBaseFrom(ConfigurationBase source)
        {
            this.Framework = source.Framework;
            this.VerifySsl = source.VerifySsl;
            this.Language = source.Language;
            this.AddHeader = source.AddHeader;
            this.ClassMapping.AddRange(source.ClassMapping);
            this.FieldMapping.AddRange(source.FieldMapping);
            this.PropertyMapping.AddRange(source.PropertyMapping);
            this.Standalone = source.Standalone;
            this.ConfigurationFilePath = source.ConfigurationFilePath;
        }
    }
}
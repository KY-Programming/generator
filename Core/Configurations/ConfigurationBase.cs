using System;
using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using Newtonsoft.Json;

namespace KY.Generator.Configurations
{
    public abstract class ConfigurationBase : IConfiguration
    {
        public virtual bool VerifySsl { get; set; } = true;

        [JsonIgnore]
        public virtual ILanguage Language { get; set; }

        [JsonProperty("Language")]
        internal virtual string LanguageKey { get; set; }

        public virtual bool AddHeader { get; set; } = true;
        public virtual bool RequireLanguage => true;

        public List<ClassMapping> ClassMapping { get; }
        public List<FieldMapping> FieldMapping { get; }
        public List<PropertyMapping> PropertyMapping { get; }
        public ConfigurationFormatting Formatting { get; set; }
        public bool BeforeBuild { get; set; }
        public Guid? OutputId { get; set; }

        protected ConfigurationBase()
        {
            this.ClassMapping = new List<ClassMapping>();
            this.FieldMapping = new List<FieldMapping>();
            this.PropertyMapping = new List<PropertyMapping>();
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
            this.Formatting = source.Formatting;
        }
    }
}
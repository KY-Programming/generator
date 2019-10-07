using System.Collections.Generic;
using System.Net;
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
        public List<Cookie> Cookies { get; }
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
            this.Cookies = new List<Cookie>();
        }

        public virtual void CopyBaseFrom(ConfigurationBase command)
        {
            this.Framework = command.Framework;
            this.VerifySsl = command.VerifySsl;
            this.Language = command.Language;
            this.AddHeader = command.AddHeader;
            this.Cookies.AddRange(command.Cookies);
            this.ClassMapping.AddRange(command.ClassMapping);
            this.FieldMapping.AddRange(command.FieldMapping);
            this.PropertyMapping.AddRange(command.PropertyMapping);
            this.Standalone = command.Standalone;
            this.ConfigurationFilePath = command.ConfigurationFilePath;
        }
    }
}
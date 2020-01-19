using System.Collections.Generic;
using KY.Generator.Mappings;
using KY.Generator.Output;
using Newtonsoft.Json;

namespace KY.Generator.Configuration
{
    public abstract class ConfigurationBase : IConfiguration
    {
        public virtual bool VerifySsl { get; set; } = true;
        public virtual bool AddHeader { get; set; } = true;

        [JsonIgnore]
        public bool SkipHeader
        {
            get => !this.AddHeader;
            set => this.AddHeader = !value;
        }

        public virtual List<ClassMapping> ClassMapping { get; }
        public virtual List<FieldMapping> FieldMapping { get; }
        public virtual List<PropertyMapping> PropertyMapping { get; }
        public virtual ConfigurationFormatting Formatting { get; set; }

        [JsonIgnore]
        public virtual bool Standalone { get; set; }

        [JsonIgnore]
        public virtual ConfigurationEnvironment Environment { get; set; }

        [JsonIgnore]
        public virtual IOutput Output { get; set; }

        public virtual bool CheckOnOverwrite { get; set; } = true;
        public virtual bool BeforeBuild { get; set; }

        protected ConfigurationBase()
        {
            this.ClassMapping = new List<ClassMapping>();
            this.FieldMapping = new List<FieldMapping>();
            this.PropertyMapping = new List<PropertyMapping>();
            this.Environment = new ConfigurationEnvironment();
            this.Formatting = new ConfigurationFormatting();
        }

        public virtual void CopyBaseFrom(IConfiguration source)
        {
            this.Environment = source.Environment;
            this.Formatting = source.Formatting;
            if (source is ConfigurationBase configurationBase)
            {
                this.VerifySsl = configurationBase.VerifySsl;
                this.AddHeader = configurationBase.AddHeader;
                this.ClassMapping.AddRange(configurationBase.ClassMapping);
                this.FieldMapping.AddRange(configurationBase.FieldMapping);
                this.PropertyMapping.AddRange(configurationBase.PropertyMapping);
                this.Standalone = configurationBase.Standalone;
            }
        }
    }
}
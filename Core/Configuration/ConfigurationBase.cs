using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Mappings;

namespace KY.Generator.Configuration
{
    public abstract class ConfigurationBase
    {
        public bool VerifySsl { get; set; } = true;
        public ILanguage Language { get; set; }
        public bool AddHeader { get; set; } = true;
        
        public List<ClassMapping> ClassMapping { get; }
        public List<FieldMapping> FieldMapping { get; }
        public List<PropertyMapping> PropertyMapping { get; }

        protected ConfigurationBase()
        {
            this.ClassMapping = new List<ClassMapping>();
            this.FieldMapping = new List<FieldMapping>();
            this.PropertyMapping = new List<PropertyMapping>();
        }
    }
}
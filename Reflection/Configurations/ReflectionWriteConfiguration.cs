using System.Collections.Generic;
using KY.Generator.Configurations;

namespace KY.Generator.Reflection.Configurations
{
    internal class ReflectionWriteConfiguration : ConfigurationBase, IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public string Using { get; set; }

        public bool FieldsToProperties
        {
            get => this.Formatting.FieldsToProperties;
            set => this.Formatting.FieldsToProperties = value;
        }

        public bool PropertiesToFields
        {
            get => this.Formatting.PropertiesToFields;
            set => this.Formatting.PropertiesToFields = value;
        }

        public bool SkipNamespace { get; set; }
        public List<string> Usings { get; }
        public bool FormatNames { get; set; }

        public ReflectionWriteConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}
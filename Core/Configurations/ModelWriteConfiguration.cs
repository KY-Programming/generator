﻿using System.Collections.Generic;

namespace KY.Generator.Configurations
{
    public class ModelWriteConfiguration : ConfigurationBase, IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool SkipNamespace { get; set; }
        public List<string> Usings { get; set; }

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

        public bool PreferInterfaces
        {
            get => this.Formatting.PreferInterfaces;
            set => this.Formatting.PreferInterfaces = value;
        }

        public bool WithOptionalProperties
        {
            get => this.Formatting.WithOptionalProperties;
            set => this.Formatting.WithOptionalProperties = value;
        }

        public bool FormatNames { get; set; }

        public ModelWriteConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}

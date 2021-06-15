﻿using KY.Generator.Configurations;

namespace KY.Generator.Json.Configurations
{
    public class JsonWriteConfiguration : ConfigurationBase, IFormattableConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool SkipNamespace { get; set; }
        public bool FieldsToProperties { get; set; }    
        public bool PropertiesToFields { get; set; }
        public bool FormatNames { get; set; }
        public bool WithReader { get; set; }

        public JsonWriteConfiguration()
        {
            this.FormatNames = true;
        }
    }
}
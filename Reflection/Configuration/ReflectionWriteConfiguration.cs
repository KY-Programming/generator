﻿using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionWriteConfiguration : ConfigurationBase, IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public string Using { get; set; }
        public bool PropertiesToFields { get; set; }
        public bool FieldsToProperties { get; set; }
        public bool SkipNamespace { get; set; }
        public List<string> Usings { get; }
        public bool FormatNames { get; set; }

        public ReflectionWriteConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}
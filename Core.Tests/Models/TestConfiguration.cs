﻿using System;
using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Languages;

namespace KY.Generator.Core.Tests.Models
{
    internal class TestConfiguration : IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool AddHeader { get; set; }
        public bool SkipNamespace { get; set; }
        public List<string> Usings { get; }
        public Guid? OutputId { get; set; }
        public bool FormatNames { get; set; }
        public ConfigurationFormatting Formatting { get; }

        public TestConfiguration()
        {
            this.Usings = new List<string>();
            this.Formatting = new ConfigurationFormatting();
        }
    }
}

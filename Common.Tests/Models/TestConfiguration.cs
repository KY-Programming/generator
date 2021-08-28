using System;
using System.Collections.Generic;
using KY.Generator.Configurations;

namespace KY.Generator.Common.Tests.Models
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

        public TestConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}

﻿using System.Collections.Generic;

namespace KY.Generator.EntityFramework.Configurations
{
    public class EntityFrameworkDataContextConfiguration
    {
        public string RelativePath { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SuppressConstructor { get; set; }
        public bool SuppressLoadTypeConfig { get; set; }
        public List<string> Usings { get; set; }
        public int CommandTimeout { get; set; } = 300;
    }
}
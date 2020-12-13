using System;
using System.Collections.Generic;

namespace KY.Generator.Configurations
{
    public interface IModelConfiguration : IFormattableConfiguration
    {
        string Name { get; set; }
        string Namespace { get; }
        string RelativePath { get; }
        bool AddHeader { get; }
        bool SkipNamespace { get; }
        List<string> Usings { get; }
        Guid? OutputId { get; }
    }
}
using System.Collections.Generic;

namespace KY.Generator.Configuration
{
    public interface IModelConfiguration : IConfigurationWithLanguage, IFormattableConfiguration
    {
        string Name { get; set; }
        string Namespace { get; }
        string RelativePath { get; }
        bool AddHeader { get; }
        bool SkipNamespace { get; }
        List<string> Usings { get; }
    }
}
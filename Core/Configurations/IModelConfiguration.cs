using System.Collections.Generic;

namespace KY.Generator.Configurations
{
    public interface IModelConfiguration
    {
        string Name { get; set; }
        string Namespace { get; }
        string RelativePath { get; }
        List<string> Usings { get; }
    }
}

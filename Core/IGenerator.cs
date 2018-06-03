using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Templates;

namespace KY.Generator
{
    public interface IGenerator
    {
        List<FileTemplate> Files { get; }
        void Generate(ConfigurationBase configuration);
    }
}
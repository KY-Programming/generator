using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.OData.Configuration;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.OData
{
    internal class ODataGenerator : IGenerator
    {
        public List<FileTemplate> Files { get; }

        public ODataGenerator()
        {
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            ODataConfiguration configuration = configurationBase as ODataConfiguration;
            if (configuration == null)
            {
                return;
            }
            this.Files.AddFile(configuration.Models?.RelativePath ?? configuration.DataContext?.RelativePath)
                .AddNamespace("Default")
                .AddClass("Dummy")
                .WithComment("Dummy class for test. Please override oData generator. Use generator.OData(...) in entry assemlby or console");
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configuration;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Tsql.Configuration;
using KY.Generator.Tsql.Entity;

namespace KY.Generator.Tsql
{
    internal class TsqlGenerator : IGenerator
    {
        public List<FileTemplate> Files { get; private set; }

        public TsqlGenerator()
        {
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            TsqlConfiguration configuration = configurationBase as TsqlConfiguration;
            if (configuration == null)
            {
                return;
            }
            TsqlEntity firstEntity = configuration.Entities.FirstOrDefault();
            this.Files.AddFile(firstEntity?.Model?.RelativePath ?? firstEntity?.Controller?.RelativePath ?? firstEntity?.DataContext.RelativePath ?? firstEntity.Enum?.RelativePath)
                .AddNamespace("Default")
                .AddClass("Dummy")
                .WithComment("Dummy class for test. Please override TSQL generator. Use generator.Tsql(...) in entry assemlby or console");
        }
    }
}
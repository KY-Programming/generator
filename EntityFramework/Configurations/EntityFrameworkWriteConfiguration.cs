using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Csharp.Languages;

namespace KY.Generator.EntityFramework.Configurations
{
    internal class EntityFrameworkWriteConfiguration : ConfigurationBase
    {
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool FormatNames { get; set; }
        public List<EntityFrameworkWriteRepositoryConfiguration> Repositories { get; set; }
        public virtual bool IsCore => false;
        public List<string> Usings { get; set; }
        public int CommandTimeout { get; set; } = 300;

        public EntityFrameworkWriteConfiguration()
        {
            this.Language = CsharpLanguage.Instance;
            this.Repositories = new List<EntityFrameworkWriteRepositoryConfiguration>();
            this.Usings= new List<string>();
        }
    }
}
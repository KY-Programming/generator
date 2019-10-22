using System.Collections.Generic;

namespace KY.Generator.EntityFramework.Configurations
{
    public class EntityFrameworkWriteRepositoryConfiguration
    { 
        public string Entity { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; set; }

        public EntityFrameworkWriteRepositoryConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}
using System.Collections.Generic;

namespace KY.Generator.EntityFramework.Configurations
{
    internal class EntityFrameworkDataContextConfiguration
    {
        public string RelativePath { get; set; }
        public string Names { get; set; }
        public string Namespace { get; set; }
        public bool SuppressConstructor { get; set; }
        public bool SuppressLoadTypeConfig { get; set; }
        public List<string> Usings { get; set; }
        public int CommandTimeout { get; set; } = 300;
        public List<EntityFrameworkStoredProcedure> StoredProcedures { get; set; }

        public EntityFrameworkDataContextConfiguration()
        {
            this.StoredProcedures = new List<EntityFrameworkStoredProcedure>();

        }
    }
}
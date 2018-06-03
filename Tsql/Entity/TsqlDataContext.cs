using System.Collections.Generic;

namespace KY.Generator.Tsql.Entity
{
    public class TsqlDataContext
    {
        public TsqlEntity Entity { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public bool SuppressConstructor { get; set; }
        public bool SuppressLoadTypeConfig { get; set; }
        public string Name { get; set; }
        public List<string> Usings { get; }
        public List<TsqlStoredProcedure> StoredProcedures { get; }

        public TsqlDataContext()
        {
            this.Usings = new List<string>();
            this.StoredProcedures = new List<TsqlStoredProcedure>();
        }
    }
}
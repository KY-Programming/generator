using System.Collections.Generic;

namespace KY.Generator.Tsql.Configurations
{
    public class TsqlReadConfiguration
    {
        public string Connection { get; set; }
        public List<TsqlReadEntity> Entities { get; }
        public List<TsqlReadStoredProcedure> StoredProcedures { get; }
        public string Schema { get; set; }
        public string Namespace { get; set; }

        public TsqlReadConfiguration()
        {
            this.Entities = new List<TsqlReadEntity>();
            this.StoredProcedures = new List<TsqlReadStoredProcedure>();
        }
    }
}

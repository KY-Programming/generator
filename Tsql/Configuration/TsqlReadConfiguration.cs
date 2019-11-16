using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Tsql.Configuration
{
    public class TsqlReadConfiguration : ConfigurationBase
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
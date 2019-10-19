using System.Collections.Generic;
using KY.Generator.Configuration;

namespace KY.Generator.Tsql.Configuration
{
    public class TsqlReadConfiguration : ConfigurationBase
    { 
        public string Connection { get; set; }
        public List<TsqlReadEntity> Entities { get; }
        public string Schema { get; set; }
        public string Namespace { get; set; }

        public TsqlReadConfiguration()
        {
            this.Entities = new List<TsqlReadEntity>();
        }
    }
}
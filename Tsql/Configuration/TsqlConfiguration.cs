using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Tsql.Entity;

namespace KY.Generator.Tsql.Configuration
{
    public class TsqlConfiguration : ConfigurationBase
    {
        public string Connection { get; set; }
        public List<TsqlEntity> Entities { get; }

        public TsqlConfiguration()
        {
            this.Entities = new List<TsqlEntity>();
        }
    }
}
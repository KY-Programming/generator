using System.Collections.Generic;

namespace KY.Generator.Tsql.Configurations
{
    public class TsqlReadEntity
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string StoredProcedure { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<TsqlReadEntityKeyAction> KeyActions { get; set; }

        public TsqlReadEntity()
        {
            this.KeyActions = new List<TsqlReadEntityKeyAction>();
        }
    }
}
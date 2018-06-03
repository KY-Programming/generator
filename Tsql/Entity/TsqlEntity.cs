using KY.Generator.Tsql.Configuration;
using KY.Generator.Tsql.OData;

namespace KY.Generator.Tsql.Entity
{
    public class TsqlEntity
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }
        public string StoredProcedure { get; set; }

        public TsqlModel Model { get; set; }
        public TsqlODataController Controller { get; set; }
        public TsqlODataRepository Repository { get; set; }
        public TsqlDataContext DataContext { get; set; }
        public TsqlEnum Enum { get; set; }

        public TsqlConfiguration Configuration { get; set; }
    }
}
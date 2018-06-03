using System.Collections.Generic;
using KY.Generator.Tsql.Entity;

namespace KY.Generator.Tsql.OData
{
    public class TsqlODataRepository
    {
        public TsqlEntity Entity { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; }
        public string BasedOn { get; set; }

        public TsqlODataRepository()
        {
            this.Usings = new List<string>();
        }
    }
}
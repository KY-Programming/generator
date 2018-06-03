using System.Collections.Generic;
using KY.Generator.Tsql.Entity;

namespace KY.Generator.Tsql.OData
{
    public class TsqlODataController
    {
        public TsqlEntity Entity { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public string Route { get; set; }
        public string BasedOn { get; set; }
        public TsqlODataControllerMethod Get { get; set; }
        public TsqlODataControllerMethod GetSingle { get; set; }
        public TsqlODataControllerMethod Post { get; set; }
        public TsqlODataControllerMethod Delete { get; set; }
        public TsqlODataControllerMethod Put { get; set; }
        public TsqlODataControllerMethod Patch { get; set; }
        public List<string> Usings { get; }

        public TsqlODataController()
        {
            this.Usings = new List<string>();
        }
    }
}
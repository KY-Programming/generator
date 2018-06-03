using System;
using System.Collections.Generic;

namespace KY.Generator.Tsql.OData
{
    public class TsqlODataControllerMethodAttribute
    {
        public string Name { get; }
        public List<Tuple<string, string>> Properties { get; }

        public TsqlODataControllerMethodAttribute(string name)
        {
            this.Name = name;
            this.Properties = new List<Tuple<string, string>>();
        }

        public TsqlODataControllerMethodAttribute WithProperty(string name, string value)
        {
            this.Properties.Add(Tuple.Create(name, value));
            return this;
        }
    }
}
using System;
using System.Collections.Generic;

namespace KY.Generator.OData.Configuration
{
    public class ODataWriteControllerMethodAttributeConfiguration
    {
        public string Name { get; }
        public List<Tuple<string, string>> Properties { get; }

        public ODataWriteControllerMethodAttributeConfiguration(string name)
        {
            this.Name = name;
            this.Properties = new List<Tuple<string, string>>();
        }

        public ODataWriteControllerMethodAttributeConfiguration WithProperty(string name, string value)
        {
            this.Properties.Add(Tuple.Create(name, value));
            return this;
        }
    }
}
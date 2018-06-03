using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataEntityType
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string FullName => $"{this.Namespace}.{this.Name}";

        public List<ODataMetaDataProperty> Properties { get; }
        public List<ODataMetaDataProperty> Keys { get; }
        public List<ODataMetaDataNavigationProperty> NavigationProperties { get; }

        public ODataMetaDataEntityType()
        {
            this.Properties = new List<ODataMetaDataProperty>();
            this.Keys = new List<ODataMetaDataProperty>();
            this.NavigationProperties = new List<ODataMetaDataNavigationProperty>();
        }
    }
}
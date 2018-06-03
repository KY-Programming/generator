using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataEntitySet
    {
        public string Name { get; set; }
        public ODataMetaDataEntityType EntityType { get; set; }
        public List<ODataMetaDataAction> Actions { get; }

        public ODataMetaDataEntitySet()
        {
            this.Actions = new List<ODataMetaDataAction>();
        }
    }
}
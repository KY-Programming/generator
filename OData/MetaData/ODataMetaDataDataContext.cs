using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataDataContext
    {
        public string Namespace { get; set; }
        public List<ODataMetaDataEntitySet> EntitySets { get; }
        public List<ODataMetaDataAction> UnboundActions { get; }

        public ODataMetaDataDataContext()
        {
            this.EntitySets = new List<ODataMetaDataEntitySet>();
            this.UnboundActions = new List<ODataMetaDataAction>();
        }
    }
}
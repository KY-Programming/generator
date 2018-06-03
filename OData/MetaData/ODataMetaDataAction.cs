using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataAction
    {
        public string Name { get; set; }
        public bool IsBound { get; set; }
        public string BoundTo { get; set; }
        public List<ODataMetaDataActionParameter> Parameters { get; }

        public ODataMetaDataAction()
        {
            this.Parameters = new List<ODataMetaDataActionParameter>();
        }
    }
}
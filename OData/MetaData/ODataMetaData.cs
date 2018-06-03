using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaData
    {
        public ODataMetaDataDataContext DataContext { get; set; }
        public List<ODataMetaDataEntityType> EntityTypes { get; }

        public ODataMetaData()
        {
            this.EntityTypes = new List<ODataMetaDataEntityType>();
        }
    }
}
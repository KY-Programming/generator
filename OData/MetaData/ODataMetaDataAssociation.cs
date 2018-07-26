using System.Collections.Generic;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataAssociation
    {
        public string Name { get; set; }
        public List<ODataMetaDataAssociationEnd> Ends { get; }

        public ODataMetaDataAssociation()
        {
            this.Ends = new List<ODataMetaDataAssociationEnd>();
        }
    }
}
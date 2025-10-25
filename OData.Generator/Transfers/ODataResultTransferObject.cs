using KY.Generator.Transfer;

namespace KY.Generator.OData.Transfers
{
    public class ODataResultTransferObject : ITransferObject
    {
        public string Result { get; set; }

        public ODataResultTransferObject(string result)
        {
            this.Result = result;
        }
    }
}
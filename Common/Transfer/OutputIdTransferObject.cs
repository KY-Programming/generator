using System;

namespace KY.Generator.Transfer
{
    public class OutputIdTransferObject : TransferObject<Guid>
    {
        public OutputIdTransferObject(Guid value)
            : base(value)
        { }
    }
}
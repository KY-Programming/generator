using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Tests.Models
{
    internal class Writer1 : ITransferWriter
    {
        public void Write(IEnumerable<ITransferObject> transferObjects, string relativePath, IOutput output)
        { }
    }
}

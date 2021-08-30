using System.Collections.Generic;
using KY.Generator.Transfer;

namespace KY.Generator.TypeScript.Transfer
{
    public class TypeScriptIndexFile : ITransferObject
    {
        public string RelativePath { get; set; }
        public List<IIndexLine> Lines { get; } = new();
    }

}

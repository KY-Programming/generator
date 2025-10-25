using System.Collections.Generic;

namespace KY.Generator.TypeScript.Transfer
{
    public class ExportIndexLine : IIndexLine
    {
        public List<string> Types { get; set; } = new();
        public string Path { get; set; }
    }
}
using System.Collections.Generic;

namespace KY.Generator.Transfer.Extensions
{
    public class FromTypeOptions : IFromTypeOptions
    {
        public List<string> ReplaceName { get; set; }
        public List<string> ReplaceWithName { get; set; }
    }
}

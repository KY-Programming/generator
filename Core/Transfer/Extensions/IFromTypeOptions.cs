using System.Collections.Generic;

namespace KY.Generator.Transfer.Extensions
{
    public interface IFromTypeOptions
    {
        List<string> ReplaceName { get; set; }
        List<string> ReplaceWithName { get; set; }
    }
}

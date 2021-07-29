using System.Collections.Generic;

namespace KY.Generator.Models
{
    public interface ICoreOptions : IOptions
    {
        IOptions Parent { get; set; }
        Dictionary<string, object> Records { get; }
        T GetRecord<T>(string key);
    }
}

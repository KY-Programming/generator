using System.Collections.Generic;

namespace KY.Generator
{
    public interface IGeneratorCommandAttribute
    {
        IEnumerable<AttributeCommandConfiguration> Commands { get; }
    }
}
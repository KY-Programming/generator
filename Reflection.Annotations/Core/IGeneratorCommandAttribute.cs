using System.Collections.Generic;

namespace KY.Generator
{
    public interface IGeneratorCommandAttribute
    {
        string Command { get; }
        List<string> Parameters { get; }
    }
}
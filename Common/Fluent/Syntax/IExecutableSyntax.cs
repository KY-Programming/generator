using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public interface IExecutableSyntax
    {
        List<IGeneratorCommand> Commands { get; }
    }
}

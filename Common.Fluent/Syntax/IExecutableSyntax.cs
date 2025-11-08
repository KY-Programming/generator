using KY.Generator.Command;

namespace KY.Generator;

public interface IExecutableSyntax
{
    List<GeneratorCommandParameters> Commands { get; }
}

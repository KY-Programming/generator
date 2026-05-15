using KY.Generator.Command;

namespace KY.Generator.Reflection.Fluent;

internal class ReflectionWriteSyntax : IExecutableSyntax, IReflectionWriteSyntax
{
    private ReflectionWriteCommandParameters? command;
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public IReflectionWriteSyntax Models(string relativePath)
    {
        this.command = new ReflectionWriteCommandParameters
        {
            RelativePath = relativePath
        };
        this.Commands.Add(this.command);
        return this;
    }
}

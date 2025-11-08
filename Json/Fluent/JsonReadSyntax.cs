using KY.Generator.Command;
using KY.Generator.Commands;

namespace KY.Generator;

public class JsonReadSyntax : IExecutableSyntax, IJsonReadSyntax
{
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public IJsonReadSyntax FromFile(string relativePath)
    {
        this.Commands.Add(new JsonReadCommandParameters
        {
            RelativePath = relativePath
        });
        return this;
    }
}

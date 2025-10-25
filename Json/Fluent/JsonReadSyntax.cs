using KY.Generator.Commands;
using KY.Generator.Syntax;

namespace KY.Generator;

public class JsonReadSyntax : ExecutableSyntax, IJsonReadSyntax
{
    public IJsonReadSyntax FromFile(string relativePath)
    {
        this.Commands.Add(new JsonReadCommandParameters
        {
            RelativePath = relativePath
        });
        return this;
    }
}

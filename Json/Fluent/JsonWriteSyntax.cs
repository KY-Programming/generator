using KY.Generator.Commands;
using KY.Generator.Syntax;

namespace KY.Generator;

public class JsonWriteSyntax : ExecutableSyntax, IJsonWriteSyntax, IJsonWriteModelSyntax, IJsonWriteModelOrReaderSyntax
{
    private readonly JsonWriteCommandParameters command = new();

    public IJsonWriteModelOrReaderSyntax Model(string relativePath, string name, string nameSpace)
    {
        this.command.RelativePath = relativePath;
        this.command.ModelName = name;
        this.command.ModelNamespace = nameSpace;
        this.Commands.Add(this.command);
        return this;
    }

    public IJsonWriteModelOrReaderSyntax FieldsToProperties()
    {
        this.command.FieldsToProperties = true;
        return this;
    }

    public IJsonWriteModelOrReaderSyntax PropertiesToFields()
    {
        this.command.PropertiesToFields = true;
        return this;
    }

    public IJsonWriteModelSyntax WithoutReader()
    {
        this.command.WithReader = false;
        return this;
    }
}

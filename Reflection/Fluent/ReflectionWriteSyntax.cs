using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent;

internal class ReflectionWriteSyntax : ExecutableSyntax, IReflectionWriteSyntax
{
    private readonly IWriteFluentSyntaxInternal syntax;
    private ReflectionWriteCommandParameters? command;

    public ReflectionWriteSyntax(IWriteFluentSyntaxInternal syntax)
    {
        this.syntax = syntax;
    }

    public IReflectionWriteSyntax Models(string relativePath)
    {
        this.command = new ReflectionWriteCommandParameters
        {
            RelativePath = relativePath
        };
        this.Commands.Add(this.command);
        return this;
    }

    public IReflectionWriteSyntax PropertiesToFields()
    {
        this.command.PropertiesToFields = true;
        return this;
    }

    public IReflectionWriteSyntax FieldsToProperties()
    {
        this.command.FieldsToProperties = true;
        return this;
    }
}

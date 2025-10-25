using KY.Core;
using KY.Generator.Angular.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent;

internal class AngularModelSyntax : IAngularModelSyntax
{
    private readonly ExecutableSyntax syntax;
    private readonly AngularModelCommandParameters command;

    public AngularModelSyntax(ExecutableSyntax syntax, AngularModelCommandParameters command)
    {
        this.syntax = syntax;
        this.command = command;
    }

    public IAngularModelSyntax FormatNames(bool value = true)
    {
        this.command.FormatNames = value;
        return this;
    }

    public IAngularModelSyntax OutputPath(string path)
    {
        this.command.RelativePath = path;
        this.syntax.Commands.OfType<AngularServiceCommandParameters>().ForEach(x => x.RelativeModelPath = path);
        return this;
    }

    public IAngularModelSyntax SkipNamespace(bool value = true)
    {
        this.command.SkipNamespace = value;
        return this;
    }

    public IAngularModelSyntax PropertiesToFields(bool value = true)
    {
        this.command.PropertiesToFields = value;
        return this;
    }

    public IAngularModelSyntax FieldsToProperties(bool value = true)
    {
        this.command.FieldsToProperties = value;
        return this;
    }

    public IAngularModelSyntax PreferInterfaces()
    {
        this.command.PreferInterfaces = true;
        return this;
    }
}

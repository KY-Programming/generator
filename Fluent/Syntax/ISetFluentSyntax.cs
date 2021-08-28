using System;

namespace KY.Generator.Syntax
{
    public interface ISetFluentSyntax
    {
        ISetFluentSyntax Strict();
        ISetFluentSyntax PropertiesToFields();
        ISetFluentSyntax FieldsToProperties();
        ISetFluentSyntax PreferInterfaces();
        ISetFluentSyntax OptionalFields();
        ISetFluentSyntax OptionalProperties();
        ISetFluentSyntax Ignore();
        ISetFluentSyntax ReplaceName(string replace, string with);
        ISetFluentSyntax OnlySubTypes();
    }
}

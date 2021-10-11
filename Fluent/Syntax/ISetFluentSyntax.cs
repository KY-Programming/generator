using System;

namespace KY.Generator.Syntax
{
    public interface ISetFluentSyntax : ISetFluentSyntax<ISetFluentSyntax>
    {
    }

    public interface ISetFluentSyntax<out T>
    {
        // TODO: Document
        T Strict();
        T PropertiesToFields();
        T FieldsToProperties();
        T PreferInterfaces();
        T OptionalFields();
        T OptionalProperties();
        T Ignore();
        T ReplaceName(string replace, string with);
        T OnlySubTypes();
        T FormatNames(bool value = true);
    }

    public interface ISetMemberFluentSyntax : ISetFluentSyntax<ISetMemberFluentSyntax>
    {
        // TODO: Document
        ISetMemberFluentSyntax Rename(string name);
        ISetMemberFluentSyntax ReturnType(Type type);
        ISetMemberFluentSyntax ReturnType(string typeName, string nameSpace, string fileName);
    }
}

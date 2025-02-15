namespace KY.Generator.Syntax;

public interface ISetFluentSyntax : ISetFluentSyntax<ISetFluentSyntax>
{ }

public interface ISetFluentSyntax<out T>
{
    // TODO: Document
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

namespace KY.Generator;

public interface IReflectionWriteSyntax : IFluentSyntax
{
    IReflectionWriteSyntax PropertiesToFields();
    IReflectionWriteSyntax FieldsToProperties();
    IReflectionWriteSyntax Models(string relativePath);
}

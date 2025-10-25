namespace KY.Generator;

public interface IReflectionWriteSyntax
{
    IReflectionWriteSyntax PropertiesToFields();
    IReflectionWriteSyntax FieldsToProperties();
    IReflectionWriteSyntax Models(string relativePath);
}

namespace KY.Generator.Reflection.Fluent
{
    public interface IReflectionWriteSyntax
    {
        IReflectionWriteSyntax PropertiesToFields();
        IReflectionWriteSyntax FieldsToProperties();
    }
}
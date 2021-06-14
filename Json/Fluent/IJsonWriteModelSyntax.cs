namespace KY.Generator
{
    public interface IJsonWriteModelSyntax
    {
        IJsonWriteModelOrReaderSyntax FieldsToProperties();
        IJsonWriteModelOrReaderSyntax PropertiesToFields();
    }
}
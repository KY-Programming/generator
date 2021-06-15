namespace KY.Generator
{
    public interface IJsonWriteModelOrReaderSyntax : IJsonWriteModelSyntax
    {
        IJsonWriteModelSyntax WithoutReader();
    }
}
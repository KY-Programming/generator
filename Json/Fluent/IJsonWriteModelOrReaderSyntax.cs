namespace KY.Generator
{
    public interface IJsonWriteModelOrReaderSyntax : IJsonWriteModelSyntax
    {
        IJsonWriteModelSyntax WithReader(string name = null, string nameSpace = null, string relativePath = null);
    }
}
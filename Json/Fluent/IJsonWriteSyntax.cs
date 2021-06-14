namespace KY.Generator
{
    public interface IJsonWriteSyntax
    {
        IJsonWriteModelOrReaderSyntax Model(string relativePath, string name, string nameSpace);
    }
}
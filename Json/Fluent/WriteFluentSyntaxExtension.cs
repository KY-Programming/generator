using KY.Generator.Syntax;

namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IJsonWriteModelOrReaderSyntax JsonModel(this IWriteFluentSyntax syntax, string relativePath, string name, string nameSpace)
        {
            return new JsonWriteSyntax((IWriteFluentSyntaxInternal)syntax).Model(relativePath, name, nameSpace);
        }
    }
}
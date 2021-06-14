using KY.Generator.Syntax;

namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static ISwitchToWriteSyntax JsonFromFile(this IReadFluentSyntax syntax, string relativePath)
        {
            return new JsonReadSyntax((IReadFluentSyntaxInternal)syntax).FromFile(relativePath);
        }
    }
}
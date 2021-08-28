using KY.Generator.Reflection.Fluent;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IReflectionWriteSyntax ReflectionModels(this IWriteFluentSyntax syntax, string relativePath)
        {
            IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
            ReflectionWriteSyntax readSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(readSyntax);
            return readSyntax.Models(relativePath);
        }
    }
}

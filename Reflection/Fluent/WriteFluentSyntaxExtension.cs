using KY.Generator.Reflection.Fluent;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IReflectionWriteSyntax ReflectionModels(this IWriteFluentSyntax syntax, string relativePath)
        {
            return new ReflectionWriteSyntax((IWriteFluentSyntaxInternal)syntax).Models(relativePath);
        }
    }
}
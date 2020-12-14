using KY.Generator.Reflection.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReflectionReadAndSwitchToWriteSyntax FromType<T>(this IReadFluentSyntax syntax)
        {
            return new ReflectionReadSyntax((FluentSyntax)syntax).FromType<T>();
        }
    }
}
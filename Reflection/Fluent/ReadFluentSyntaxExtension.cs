using KY.Generator.Reflection.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        /// <inheritdoc cref="IReflectionReadSyntax.FromType{T}"/>
        public static IReflectionFromTypeOrReflectionReadSyntax FromType<T>(this IReadFluentSyntax syntax)
        {
            return new ReflectionReadSyntax((IReadFluentSyntaxInternal)syntax).FromType<T>();
        }
    }
}
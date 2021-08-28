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
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            ReflectionReadSyntax readSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(readSyntax);
            return readSyntax.FromType<T>();
        }
    }
}

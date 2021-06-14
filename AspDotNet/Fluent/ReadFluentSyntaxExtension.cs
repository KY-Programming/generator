using KY.Generator.AspDotNet.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        /// <inheritdoc cref="IAspDotNetReadSyntax.FromController{T}"/>
        public static IAspDotNetControllerOrReadSyntax FromController<T>(this IReadFluentSyntax syntax)
        {
            return new AspDotNetReadSyntax((IReadFluentSyntaxInternal)syntax).FromController<T>();
        }

        /// <inheritdoc cref="IAspDotNetReadSyntax.FromHub{T}"/>
        public static IAspDotNetHubOrReadSyntax FromHub<T>(this IReadFluentSyntax syntax)
        {
            return new AspDotNetReadSyntax((IReadFluentSyntaxInternal)syntax).FromHub<T>();
        }
    }
}
using System;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReadFluentOrSwitchToWriteSyntax Json(this IReadFluentSyntax syntax, Action<IJsonReadSyntax> action)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            JsonReadSyntax readSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(readSyntax);
            action(readSyntax);
            return internalSyntax;
        }
    }
}

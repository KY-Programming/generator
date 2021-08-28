using System;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IWriteFluentSyntax Json(this IWriteFluentSyntax syntax, Action<IJsonWriteSyntax> action)
        {
            IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
            JsonWriteSyntax writeSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(writeSyntax);
            action(writeSyntax);
            return internalSyntax;
        }
    }
}

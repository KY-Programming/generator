using System;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IWriteFluentSyntax Json(this IWriteFluentSyntax syntax, Action<IJsonWriteSyntax> action)
        {
            action(new JsonWriteSyntax((IWriteFluentSyntaxInternal)syntax));
            return syntax;
        }
    }
}
using System;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReadFluentOrSwitchToWriteSyntax Json(this IReadFluentSyntax syntax, Action<IJsonReadSyntax> action)
        {
            action(new JsonReadSyntax((IReadFluentSyntaxInternal)syntax));
            return (IReadFluentSyntaxInternal)syntax;
        }
    }
}
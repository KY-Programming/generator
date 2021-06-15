using System;
using KY.Generator.Angular.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IWriteFluentSyntax Angular(this IWriteFluentSyntax syntax, Action<IAngularWriteSyntax> action)
        {
            action(new AngularWriteSyntax((IWriteFluentSyntaxInternal)syntax));
            return syntax;
        }
    }
}
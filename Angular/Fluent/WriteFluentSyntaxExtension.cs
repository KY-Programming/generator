using System;
using KY.Core;
using KY.Generator.Angular.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        public static IWriteFluentSyntax Angular(this IWriteFluentSyntax syntax, Action<IAngularWriteSyntax> action)
        {
            IFluentInternalSyntax internalSyntax = syntax.CastTo<IFluentInternalSyntax>();
            AngularWriteSyntax writeSyntax = internalSyntax.Resolver.Create<AngularWriteSyntax>();
            internalSyntax.Syntaxes.Add(writeSyntax);
            action(writeSyntax);
            return syntax;
        }
    }
}

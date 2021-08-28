using System;
using KY.Core;
using KY.Generator.AspDotNet.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReadFluentOrSwitchToWriteSyntax AspDotNet(this IReadFluentSyntax syntax, Action<IAspDotNetReadSyntax> action)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            AspDotNetReadSyntax readSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(readSyntax);
            action(readSyntax);
            return (IReadFluentSyntaxInternal)syntax;
        }
    }
}

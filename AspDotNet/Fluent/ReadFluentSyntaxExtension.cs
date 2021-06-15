using System;
using KY.Generator.AspDotNet.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReadFluentOrSwitchToWriteSyntax AspDotNet(this IReadFluentSyntax syntax, Action<IAspDotNetReadSyntax> action)
        {
            action(new AspDotNetReadSyntax((IReadFluentSyntaxInternal)syntax));
            return (IReadFluentSyntaxInternal)syntax;
        }
    }
}
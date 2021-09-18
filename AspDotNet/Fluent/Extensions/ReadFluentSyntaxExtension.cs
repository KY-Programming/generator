using System;
using KY.Core;
using KY.Generator.AspDotNet.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        /// <summary>
        /// Executes the ASP.NET read commands. Use at least one command!
        /// </summary>
        public static IReadFluentSyntax AspDotNet(this IReadFluentSyntax syntax, Action<IAspDotNetReadSyntax> action)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            AspDotNetReadSyntax readSyntax = internalSyntax.Resolver.Create<AspDotNetReadSyntax>();
            internalSyntax.Syntaxes.Add(readSyntax);
            action(readSyntax);
            readSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(AspDotNet)} action requires at least one command. E.g. '.{nameof(AspDotNet)}(read => read.{nameof(IAspDotNetReadSyntax.FromController)}<MyController>())'");
            return internalSyntax;
        }
    }
}

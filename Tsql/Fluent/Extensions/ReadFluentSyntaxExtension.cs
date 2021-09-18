using System;
using KY.Core;
using KY.Generator.Syntax;
using KY.Generator.Tsql.Fluent;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        /// <summary>
        /// Executes the Tsql read commands. Use at least one command!
        /// </summary>
        // TODO: Use action instead to insert connectionString parameter here
        public static IReadFluentSyntax Tsql(this IReadFluentSyntax syntax, string connectionString, Action<ITsqlReadSyntax> action)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            TsqlReadSyntax readSyntax = new(internalSyntax, connectionString);
            internalSyntax.Syntaxes.Add(readSyntax);
            action(readSyntax);
            readSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Tsql)} action requires at least one command. E.g. '.{nameof(Tsql)}(read => read.{nameof(ITsqlReadSyntax.FromTable)}(...))'");
            return syntax;
        }
    }
}

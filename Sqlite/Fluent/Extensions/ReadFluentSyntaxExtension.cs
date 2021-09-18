using System;
using KY.Core;
using KY.Generator.Sqlite.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        /// <summary>
        /// Executes the Sqlite read commands. Use at least one command!
        /// </summary>
        public static IReadFluentSyntax Sqlite(this IReadFluentSyntax syntax, Action<ISqliteReadSyntax> action)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            SqliteReadSyntax readSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(readSyntax);
            action(readSyntax);
            readSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Sqlite)} action requires at least one command. E.g. '.{nameof(Sqlite)}(read => read.{nameof(ISqliteReadSyntax.UseConnectionString)}(...))'");
            return internalSyntax;
        }
    }
}

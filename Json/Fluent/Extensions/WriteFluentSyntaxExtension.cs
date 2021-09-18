using System;
using KY.Core;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        /// <summary>
        /// Executes the JSON write commands. Use at least one command!
        /// </summary>
        public static IWriteFluentSyntax Json(this IWriteFluentSyntax syntax, Action<IJsonWriteSyntax> action)
        {
            IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
            JsonWriteSyntax writeSyntax = new(internalSyntax);
            internalSyntax.Syntaxes.Add(writeSyntax);
            action(writeSyntax);
            writeSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Json)} action requires at least one command. E.g. '.{nameof(Json)}(write => write.{nameof(IJsonWriteSyntax.Model)}(...))'");
            return internalSyntax;
        }
    }
}

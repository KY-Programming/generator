using System;
using KY.Generator.Sqlite.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static IReadFluentOrSwitchToWriteSyntax Sqlite(this IReadFluentSyntax syntax, Action<ISqliteReadSyntax> action)
        {
            action(new SqliteReadSyntax((IReadFluentSyntaxInternal)syntax));
            return (IReadFluentSyntaxInternal)syntax;
        }
    }
}

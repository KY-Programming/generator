using KY.Generator.Syntax;

namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static ITsqlReadTableSyntax Tsql(this IReadFluentSyntax syntax, string connectionString)
        {
            IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
            TsqlReadSyntax readSyntax = new(internalSyntax, connectionString);
            internalSyntax.Syntaxes.Add(readSyntax);
            return readSyntax;
        }
    }
}

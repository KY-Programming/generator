using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        /// <summary>
        /// Generates code, valid for TypeScripts strict mode
        /// </summary>
        public static IWriteFluentSyntax Strict(this IWriteFluentSyntax syntax, bool value = true)
        {
            IOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Current;
            options.Strict = value;
            return syntax;
        }

        /// <summary>
        /// Does not generate index.ts files anymore
        /// </summary>
        public static IWriteFluentSyntax NoIndex(this IWriteFluentSyntax syntax)
        {
            IOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Current;
            options.NoIndex = true;
            return syntax;
        }
    }
}

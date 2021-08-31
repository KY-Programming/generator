using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator
{
    public static class WriteFluentSyntaxExtension
    {
        /// <summary>
        /// Generates code, valid for strict mode
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IWriteFluentSyntax Strict(this IWriteFluentSyntax syntax, bool value = true)
        {
            IOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Current;
            options.Strict = value;
            return syntax;
        }

        public static IWriteFluentSyntax NoIndex(this IWriteFluentSyntax syntax)
        {
            IOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Current;
            options.NoIndex = true;
            return syntax;
        }
    }
}

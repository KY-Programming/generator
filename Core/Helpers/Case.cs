namespace KY.Generator
{
    public static class Case
    {
        /// <summary>
        /// camelCase (all characters will be cased, special chars will be removed)
        /// </summary>
        public static string CamelCase => nameof(CamelCase).ToLowerInvariant();

        /// <summary>
        /// PascalCase (all characters will be cased, special chars will be removed)
        /// </summary>
        public static string PascalCase => nameof(PascalCase).ToLowerInvariant();

        /// <summary>
        /// snake_case (all characters will be cased, special chars except underscore will be removed)
        /// </summary>
        public static string SnakeCase => nameof(SnakeCase).ToLowerInvariant();

        /// <summary>
        /// Darwin_Case (all characters will be cased, special chars except underscore will be removed)
        /// </summary>
        public static string DarwinCase => nameof(DarwinCase).ToLowerInvariant();

        /// <summary>
        /// kebab-case (all characters will be cased, special chars except dash will be removed)
        /// </summary>
        public static string KebabCase => nameof(KebabCase).ToLowerInvariant();

        /// <summary>
        /// firstCharToLower (only first char will be cased)
        /// </summary>
        public static string FirstCharToLower => nameof(FirstCharToLower).ToLowerInvariant();

        /// <summary>
        /// FirstCharToUpper (only first char will be cased)
        /// </summary>
        public static string FirstCharToUpper => nameof(FirstCharToUpper).ToLowerInvariant();
    }
}
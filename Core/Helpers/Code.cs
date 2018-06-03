using System;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static partial class Code
    {
        public static ILanguageList Languages => null;

        public static MultilineCodeFragment Multiline()
        {
            return new MultilineCodeFragment();
        }
    }
}
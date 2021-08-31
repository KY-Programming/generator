using System.Collections.Generic;
using KY.Generator.Languages;

namespace KY.Generator
{
    public interface IOptions
    {
        bool PropertiesToFields { get; set; }
        bool FieldsToProperties { get; set; }
        bool PreferInterfaces { get; set; }
        bool OptionalFields { get; set; }
        bool OptionalProperties { get; set; }
        bool Ignore { get; set; }
        bool FormatNames { get; set; }
        bool WithOptionalProperties { get; set; }
        Dictionary<string, string> ReplaceName { get; }
        FormattingOptions Formatting { get; }
        ILanguage Language { get; set; }
        bool AddHeader { get; set; }
        bool SkipNamespace { get; set; }
        bool OnlySubTypes { get; set; }

        // TODO: Should be moved to ITypeScriptOptions
        bool Strict { get; set; }
        bool IsStrictSet { get; }
        bool NoIndex { get; set; }
        // TODO-END
    }
}

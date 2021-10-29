using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Transfer;

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
        string Rename { get; set; }
        TypeTransferObject ReturnType { get; set; }
        string Formatter { get; set; }
        bool ForceOverwrite { get; set; }

        // TODO: Should be moved to ITypeScriptOptions
        bool Strict { get; set; }
        bool IsStrictSet { get; }
        bool NoIndex { get; set; }
        bool ForceIndex { get; set; }
        // TODO-END
    }
}

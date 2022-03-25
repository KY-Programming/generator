using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Transfer;

namespace KY.Generator
{
    public class OptionsPart
    {
        public bool? Strict { get; set; }
        public bool? PropertiesToFields { get; set; }
        public bool? FieldsToProperties { get; set; }
        public bool? PreferInterfaces { get; set; }
        public bool? OptionalFields { get; set; }
        public bool? OptionalProperties { get; set; }
        public bool? Ignore { get; set; }
        public bool? FormatNames { get; set; }
        public bool? WithOptionalProperties { get; set; }
        public bool? AddHeader { get; set; }
        public bool? SkipNamespace { get; set; }
        public bool? OnlySubTypes { get; set; }
        public bool? NoIndex { get; set; }
        public bool? ForceIndex { get; set; }
        public Dictionary<string, string> ReplaceName { get; } = new();
        public ILanguage Language { get; set; }
        public string Rename { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public string Formatter { get; set; }
        public bool? ForceOverwrite { get; set; }
        public bool? NoOptional { get; set; }
    }
}

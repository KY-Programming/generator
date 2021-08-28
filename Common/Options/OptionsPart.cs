using System;
using System.Collections.Generic;
using KY.Generator.Languages;

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
        public bool? SkipSelf { get; set; }
        public Dictionary<string, string> ReplaceName { get; } = new();
        public ILanguage Language { get; set; }
    }
}

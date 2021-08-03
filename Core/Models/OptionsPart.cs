using System.Collections.Generic;

namespace KY.Generator.Models
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
        public Dictionary<string, string> ReplaceName { get; set; }
    }
}

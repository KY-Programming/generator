using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("[{Name}({Code})]")]
    public class AttributeTemplate : ICodeFragment
    {
        public string Name { get; }
        public ICodeFragment[] Code { get; private set; }
        public bool HasValue => this.Code != null && this.Code.Length > 0;
        public Dictionary<string, object> Properties { get; }
        public bool IsInline { get; set; }

        public AttributeTemplate(string name, params ICodeFragment[] code)
        {
            this.Name = name;
            this.Code = code;
            this.Properties = new Dictionary<string, object>();
        }

        public AttributeTemplate Property(string name, object value)
        {
            this.Properties.Add(name, value);
            return this;
        }
    }
}

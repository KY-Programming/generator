using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("[{Name}({Code})]")]
    public class AttributeTemplate : CodeFragment
    {
        public string Name { get; }
        public CodeFragment Code { get; private set; }
        public bool HasValue => this.Code != null;
        public Dictionary<string, object> Properties { get; }

        public AttributeTemplate(string name, CodeFragment code = null)
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

        public AttributeTemplate SetCode(CodeFragment code)
        {
            this.Code = code;
            return this;
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("[{Name}({Code})]")]
    public class AttributeTemplate : ICodeFragment
    {
        public string Name { get; }
        public ICodeFragment Code { get; private set; }
        public bool HasValue => this.Code != null;
        public Dictionary<string, object> Properties { get; }

        public AttributeTemplate(string name, ICodeFragment code = null)
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

        public AttributeTemplate SetCode(ICodeFragment code)
        {
            this.Code = code;
            return this;
        }
    }
}
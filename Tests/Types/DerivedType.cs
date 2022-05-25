using System.Collections.Generic;
using KY.Generator;

namespace Types
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class DerivedType : BaseType
    {
        public string StringProperty { get; set; }
        public new string NewStringProperty { get; set; }
        public override string VirtualStringProperty { get; set; }
    }

    public class BaseType
    {
        public string NewStringProperty { get; set; }
        public virtual string VirtualStringProperty { get; set; }
    }
}

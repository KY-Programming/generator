using System.Collections.Generic;
using KY.Generator;

namespace Types
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class DerivedFromAbstractType : AbstractType
    {
        public string StringProperty { get; set; }
        public override string AbstractStringProperty { get; set; }
    }

    public abstract class AbstractType
    {
        public abstract string AbstractStringProperty { get; set; }
    }
}

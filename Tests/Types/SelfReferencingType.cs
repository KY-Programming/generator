using System.Collections.Generic;
using KY.Generator;

namespace Types
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class SelfReferencingType
    {
        public string StringProperty { get; set; }
        public SelfReferencingType SelfProperty { get; set; }
        public List<SelfReferencingType> SelfList { get; set; }
    }
}
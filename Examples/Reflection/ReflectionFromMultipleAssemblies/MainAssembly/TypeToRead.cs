using KY.Generator;
using SecondAssembly;

namespace MainAssembly
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
        public SecondType SecondTypeProperty { get; set; }
    }
}
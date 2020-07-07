using KY.Generator;

namespace ReflectionFromAttributes
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
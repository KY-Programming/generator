using KY.Generator;

namespace ReflectionFromNet5
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
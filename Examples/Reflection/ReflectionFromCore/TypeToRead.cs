using KY.Generator;

namespace ReflectionFromCore
{
    [GenerateAngularModel("Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
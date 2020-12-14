using KY.Generator;

namespace ReflectionFromStandard
{
    [GenerateAngularModel("Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
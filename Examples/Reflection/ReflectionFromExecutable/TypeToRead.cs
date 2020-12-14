using KY.Generator;

namespace ReflectionFromExecutable
{
    [GenerateAngularModel("Output")]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
using KY.Generator.Reflection;

namespace ReflectionFromAttributes
{
    [Generate]
    public class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
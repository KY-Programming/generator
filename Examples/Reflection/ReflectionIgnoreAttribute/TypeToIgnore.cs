using KY.Generator;

namespace ReflectionIgnoreAttribute
{
    [GenerateIgnore]
    public class TypeToIgnore
    {
        public string StringProperty { get; set; }
    }
}
using KY.Generator;

namespace ReflectionIgnoreAttribute
{
    [GenerateOnlySubTypes]
    public class GenerateOnlySubTypes
    {
        public GenerateOnlySubTypesSubType Property1 { get; }
        public TypeToIgnore Property2 { get; }
    }

    public class GenerateOnlySubTypesSubType
    {
        public string Property { get; }
    }
}

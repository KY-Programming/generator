using KY.Generator;

namespace ReflectionFromIndex
{
    [GenerateIndex(OutputLanguage.Inherit, "Output")]
    internal class Index
    {
        public TypeToRead type1;
        public AnotherType type2;
        public IgnoredType type3;
    }
}
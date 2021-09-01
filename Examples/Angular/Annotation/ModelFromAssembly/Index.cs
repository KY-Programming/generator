using KY.Generator;

namespace ModelFromAssembly
{
    [GenerateAngularModel("Output"), GenerateOnlySubTypes]
    internal class Index
    {
        public TypeToRead type1;
        public AnotherType type2;
        public IgnoredType type3;
    }
}

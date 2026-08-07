using KY.Generator;

namespace ModelFromAssembly
{
    /// <summary>
    /// Marker class that pulls the referenced types into generation without being generated itself.
    /// [GenerateOnlySubTypes] emits the member types only - no index.ts entry is written for Index.
    /// The output can be configured here or in AssemblyInfo.cs.
    /// </summary>
    [GenerateAngularModel, GenerateOnlySubTypes]
    internal class Index
    {
        public TypeToRead Type1 { get; set; }
        public AnotherType Type2 { get; set; }
        public IgnoredType Type3 { get; set; }
    }
}

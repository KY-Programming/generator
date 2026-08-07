using KY.Generator;

namespace FromModel
{
    /// <summary>
    /// The output can be configured here via the attributes or in AssemblyInfo.cs.
    /// </summary>
    [GenerateAngularModel]
    internal class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}

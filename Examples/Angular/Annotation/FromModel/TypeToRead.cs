using KY.Generator;

namespace FromModel
{
    [GenerateAngularModel("Output/Models")]
    internal class TypeToRead
    {
        public string StringProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}

using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Third")]
    [GenerateWithoutHeader]
    public class ThirdType
    {
        public string StringProperty { get; set; }
    }
}

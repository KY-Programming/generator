using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Third")]
    [GenerateNoHeader]
    public class ThirdType
    {
        public string StringProperty { get; set; }
    }
}

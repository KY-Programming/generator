using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Third")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class ThirdType
    {
        public string StringProperty { get; set; }
    }
}
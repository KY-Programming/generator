using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Second")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class SecondType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}
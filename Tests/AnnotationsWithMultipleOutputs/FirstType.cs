using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\First")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class FirstType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}
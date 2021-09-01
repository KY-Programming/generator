using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\First")]
    [GenerateNoHeader]
    public class FirstType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}

using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Second")]
    [GenerateNoHeader]
    public class SecondType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}

using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\Second")]
    public class SecondType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}
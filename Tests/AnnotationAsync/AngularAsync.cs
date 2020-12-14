using KY.Generator;

namespace AnnotationAsync
{
    [GenerateAngularModel("Output")]
    [GenerateOption(GenerateOption.SkipHeader)]
    [GenerateAsync]
    public class AngularAsync
    {
        public string Property { get; set; }
    }
}
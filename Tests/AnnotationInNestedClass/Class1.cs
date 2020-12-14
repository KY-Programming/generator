using KY.Generator;

namespace AnnotationInNestedClass
{
    public class Class1
    {
        [Generate(OutputLanguage.TypeScript, "Output")]
        [GenerateOption(GenerateOption.SkipHeader)]
        public class Class2
        { }
    }
}
using KY.Generator;

namespace AnnotationInNestedClass
{
    public class Class1
    {
        [Generate(OutputLanguage.TypeScript, "Output")]
        public class Class2
        { }
    }
}
using System;
using KY.Generator;

namespace AnnotationAsyncAssembly
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class Class1
    {
        public string StringProperty { get; set; }
    }
}

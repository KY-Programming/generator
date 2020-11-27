using KY.Generator;

namespace AnnotationAsync
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class NotAsyncType
    {
        public string StringProperty { get; set; }
    }
}
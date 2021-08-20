using KY.Generator;

namespace AnnotationAsync
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class NotAsyncType
    {
        public string StringProperty { get; set; }
    }
}

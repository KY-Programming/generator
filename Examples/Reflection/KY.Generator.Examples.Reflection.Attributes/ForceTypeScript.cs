using KY.Generator.Reflection;

namespace KY.Generator.Examples.Reflection.Attributes
{
    [Generate(OutputLanguage.TypeScript, "..\\..\\..\\KY.Generator.Examples.Reflection.Attributes\\TypeScript-Output")]
    public class ForceTypeScript
    {
        public string StringProperty { get; set; }
        public short ShortProperty { get; set; }
        public ushort UShortProperty { get; set; }
        public int IntProperty { get; set; }
    }
}
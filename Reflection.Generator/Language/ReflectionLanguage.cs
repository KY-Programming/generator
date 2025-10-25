using KY.Generator.Languages;

namespace KY.Generator.Reflection.Language
{
    public class ReflectionLanguage : EmptyLanguage
    {
        public static ReflectionLanguage Instance { get; } = new ReflectionLanguage();

        public override string Name => "Reflection";

        private ReflectionLanguage()
        { }
    }
}
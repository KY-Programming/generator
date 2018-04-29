using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static class Csharp
    {
        public static CsharpLanguage Language { get; } = new CsharpLanguage();

        public static CsharpTemplate Code(string code)
        {
            return new CsharpTemplate(code);
        }

        public static NullCoalescingOperatorTemplate NullCoalescing(CodeFragment codeLeft, CodeFragment codeRight)
        {
            return new NullCoalescingOperatorTemplate(codeLeft, codeRight);
        }
    }
}
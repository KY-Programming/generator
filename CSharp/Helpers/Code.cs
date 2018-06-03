using KY.Generator.Csharp.Languages;
using KY.Generator.Csharp.Templates;
using KY.Generator.Templates;

namespace KY.Generator.Csharp
{
    public static class Code
    {
        public static CsharpLanguage Language { get; } = new CsharpLanguage();

        public static CsharpTemplate Csharp(string code)
        {
            return new CsharpTemplate(code);
        }

        public static NullCoalescingOperatorTemplate NullCoalescing(ICodeFragment codeLeft, ICodeFragment codeRight)
        {
            return new NullCoalescingOperatorTemplate(codeLeft, codeRight);
        }
    }
}
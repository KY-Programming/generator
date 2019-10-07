using KY.Generator.Csharp.Templates;
using KY.Generator.Templates;

namespace KY.Generator.Csharp
{
    public static class CsharpCode
    {
        public static CsharpTemplate Csharp(this Code _, string code)
        {
            return new CsharpTemplate(code);
        }

        public static NullCoalescingOperatorTemplate NullCoalescing(this Code _, ICodeFragment codeLeft, ICodeFragment codeRight)
        {
            return new NullCoalescingOperatorTemplate(codeLeft, codeRight);
        }
    }
}
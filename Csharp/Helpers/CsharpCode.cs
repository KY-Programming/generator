using System.Text;
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

        public static UsingDeclarationTemplate Using(this Code _, ICodeFragment code)
        {
            return new UsingDeclarationTemplate(code);
        }

        public static VerbatimStringTemplate VerbatimString(this Code _, string value)
        {
            return new VerbatimStringTemplate(value);
        }

        public static VerbatimStringTemplate VerbatimString(this Code _, StringBuilder value)
        {
            return new VerbatimStringTemplate(value?.ToString());
        }

        public static YieldReturnTemplate YieldReturn(this Code _, ICodeFragment code)
        {
            return new YieldReturnTemplate(code);
        }
    }
}

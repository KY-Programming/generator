using KY.Generator.Templates;

namespace KY.Generator.Csharp.Templates
{
    public class NullCoalescingOperatorTemplate : ICodeFragment
    {
        public ICodeFragment LeftCode { get; }
        public ICodeFragment RightCode { get; }

        public NullCoalescingOperatorTemplate(ICodeFragment leftCode, ICodeFragment rightCode)
        {
            this.LeftCode = leftCode;
            this.RightCode = rightCode;
        }
    }
}
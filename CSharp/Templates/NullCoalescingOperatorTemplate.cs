namespace KY.Generator.Templates
{
    public class NullCoalescingOperatorTemplate : CodeFragment
    {
        public CodeFragment LeftCode { get; }
        public CodeFragment RightCode { get; }

        public NullCoalescingOperatorTemplate(CodeFragment leftCode, CodeFragment rightCode)
        {
            this.LeftCode = leftCode;
            this.RightCode = rightCode;
        }
    }
}
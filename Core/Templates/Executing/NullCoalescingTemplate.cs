namespace KY.Generator.Templates
{
    public class NullCoalescingTemplate : ChainedCodeFragment
    {
        public ICodeFragment Code { get; }

        public NullCoalescingTemplate(ICodeFragment code = null)
        {
            this.Code = code;
        }

        public override string Separator => "";
    }
}

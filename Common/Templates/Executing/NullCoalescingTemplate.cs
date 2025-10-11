namespace KY.Generator.Templates;

public class NullCoalescingTemplate : ChainedCodeFragment
{
    public ICodeFragment Code { get; }

    public override string Separator => "";

    public NullCoalescingTemplate(ICodeFragment code = null)
    {
        this.Code = code;
    }

    public override object Clone()
    {
        return this.CloneTo(new NullCoalescingTemplate(this.Code));
    }
}

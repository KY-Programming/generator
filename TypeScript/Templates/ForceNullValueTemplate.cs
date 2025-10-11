using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates;

public class ForceNullValueTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new ForceNullValueTemplate());
    }
}

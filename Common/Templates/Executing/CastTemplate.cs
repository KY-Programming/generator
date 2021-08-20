namespace KY.Generator.Templates
{
    public class CastTemplate : ChainedCodeFragment
    {
        public TypeTemplate Type { get; }
        public override string Separator => "";

        public CastTemplate(TypeTemplate type)
        {
            this.Type = type;
        }
    }
}

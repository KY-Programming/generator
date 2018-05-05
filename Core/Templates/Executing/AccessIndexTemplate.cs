namespace KY.Generator.Templates
{
    public class AccessIndexTemplate : ChainedCodeFragment
    {
        public CodeFragment Code { get; }
        public override string Separator => string.Empty;

        public AccessIndexTemplate(CodeFragment code)
        {
            this.Code = code;
        }
    }
}
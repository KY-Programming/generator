namespace KY.Generator.Templates
{
    public class AssignTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public ICodeFragment Code { get; }

        public AssignTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}
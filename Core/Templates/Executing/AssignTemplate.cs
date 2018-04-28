namespace KY.Generator.Templates
{
    public class AssignTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public CodeFragment Code { get; }

        public AssignTemplate(CodeFragment code)
        {
            this.Code = code;
        }
    }
}
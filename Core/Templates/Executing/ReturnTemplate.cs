namespace KY.Generator.Templates
{
    public class ReturnTemplate : CodeFragment
    {
        public CodeFragment Code { get; }

        public ReturnTemplate(CodeFragment code)
        {
            this.Code = code;
        }
    }
}
namespace KY.Generator.Templates
{
    public class ReturnTemplate : ICodeFragment
    {
        public ICodeFragment Code { get; }

        public ReturnTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}
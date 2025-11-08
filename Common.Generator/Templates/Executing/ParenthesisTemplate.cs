namespace KY.Generator.Templates
{
    public class ParenthesisTemplate : ICodeFragment
    {
        public ICodeFragment Code { get; }

        public ParenthesisTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}

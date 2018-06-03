namespace KY.Generator.Templates
{
    public class LambdaTemplate : ICodeFragment
    {
        public string ParameterName { get; }
        public ICodeFragment Code { get; }

        public LambdaTemplate(string parameterName, ICodeFragment code)
        {
            this.ParameterName = parameterName;
            this.Code = code;
        }
    }
}
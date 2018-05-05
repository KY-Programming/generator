namespace KY.Generator.Templates
{
    public class LambdaTemplate : CodeFragment
    {
        public string ParameterName { get; }
        public CodeFragment Code { get; }

        public LambdaTemplate(string parameterName, CodeFragment code)
        {
            this.ParameterName = parameterName;
            this.Code = code;
        }
    }
}
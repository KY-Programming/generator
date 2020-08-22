using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class LambdaTemplate : ICodeFragment
    {
        public List<ParameterTemplate> Parameters { get; }
        public string ParameterName { get; }
        public ICodeFragment Code { get; }

        public LambdaTemplate(string parameterName, ICodeFragment code)
        {
            this.ParameterName = parameterName;
            this.Code = code;
        }

        public LambdaTemplate(List<ParameterTemplate> parameters, ICodeFragment code)
            : this((string)null, code)
        {
            this.Parameters = parameters;
        }
    }
}
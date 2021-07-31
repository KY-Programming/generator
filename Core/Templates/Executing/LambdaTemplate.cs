using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Templates
{
    public class LambdaTemplate : ICodeFragment
    {
        public List<ParameterTemplate> Parameters { get; }
        public List<string> ParameterNames { get; }
        public ICodeFragment Code { get; }

        public LambdaTemplate(IEnumerable<string> parameterNames, ICodeFragment code)
        {
            this.ParameterNames = parameterNames.Where(x => x != null).ToList();
            this.Code = code;
        }

        public LambdaTemplate(IEnumerable<ParameterTemplate> parameters, ICodeFragment code)
        {
            this.Parameters = parameters.Where(x => x?.Name != null).ToList();
            this.Code = code;
        }
    }
}

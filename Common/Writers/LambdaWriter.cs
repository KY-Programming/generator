﻿using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class LambdaWriter : ITemplateWriter
    {
        private readonly IOptions options;

        public LambdaWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            LambdaTemplate template = (LambdaTemplate)fragment;
            output.Add("(");
            if (template.Parameters != null)
            {
                output.Add(template.Parameters, ", ");
            }
            else if (template.ParameterNames?.Count == 1)
            {
                output.Add(template.ParameterNames[0]);
            }
            else if (template.ParameterNames?.Count > 1)
            {
                output.Add(string.Join(", ", template.ParameterNames));
            }
            output.Add(")")
                  .Add(" =>");
            if (template.Code is MultilineCodeFragment)
            {
                output.StartBlock();
            }
            else
            {
                output.Add(" ");
            }
            output.Add(template.Code);
            if (template.Code is MultilineCodeFragment)
            {
                output.EndBlock(this.options.Formatting.StartBlockInNewLine);
            }
        }
    }
}

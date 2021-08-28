using System;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class ThrowWriter : ITemplateWriter
    {
        private readonly IOptions options;

        public ThrowWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ThrowTemplate template = (ThrowTemplate)fragment;
            if (template.Type.Name == nameof(ArgumentOutOfRangeException))
            {
                output.Add("throw new Error(")
                      .Add(template.Parameters[2])
                      .Add(" + ")
                      .Add(this.options.Formatting.Quote)
                      .Add(" Actual value: ")
                      .Add(this.options.Formatting.Quote)
                      .Add(" + ")
                      .Add(template.Parameters[1])
                      .Add(")");
            }
            else
            {
                output.Add("throw new Error(")
                      .Add(template.Parameters)
                      .Add(")");
            }
        }
    }
}

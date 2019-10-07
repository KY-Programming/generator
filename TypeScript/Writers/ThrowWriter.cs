using System;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class ThrowWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ThrowTemplate template = (ThrowTemplate)fragment;
            if (template.Type.Name == nameof(ArgumentOutOfRangeException))
            {
                output.Add("throw new Error(")
                      .Add(template.Parameters[2])
                      .Add(" + \" Actual value: \" + ")
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
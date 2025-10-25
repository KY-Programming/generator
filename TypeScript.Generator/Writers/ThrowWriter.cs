using System;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class ThrowWriter : ITemplateWriter
{
    private readonly Options options;

    public ThrowWriter(Options options)
    {
        this.options = options;
    }

    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        ThrowTemplate template = (ThrowTemplate)fragment;
        if (template.Type.Name == nameof(ArgumentOutOfRangeException))
        {
            output.Add("throw new Error(")
                  .Add(template.Parameters[2])
                  .Add(" + ")
                  .Add(generatorOptions.Formatting.Quote)
                  .Add(" Actual value: ")
                  .Add(generatorOptions.Formatting.Quote)
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

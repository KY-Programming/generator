using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using StringWriter = KY.Generator.Writers.StringWriter;

namespace KY.Generator.Csharp.Writers;

public class VerbatimStringWriter : StringWriter
{
    private readonly Options options;

    public VerbatimStringWriter(Options options)
        : base(options)
    {
        this.options = options;
    }

    public override void Write(ICodeFragment fragment, IOutputCache output)
    {
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        VerbatimStringTemplate template = (VerbatimStringTemplate)fragment;
        output.Add("@")
              .Add(generatorOptions.Formatting.Quote)
              .Add(template.Value, true)
              .Add(generatorOptions.Formatting.Quote);
    }
}

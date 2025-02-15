using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers;

public class StringWriter : ITemplateWriter
{
    private readonly Options options;

    public StringWriter(Options options)
    {
        this.options = options;
    }

    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        StringTemplate template = (StringTemplate)fragment;
        output.Add(generatorOptions.Formatting.Quote)
              .Add(template.Value)
              .Add(generatorOptions.Formatting.Quote);
    }
}

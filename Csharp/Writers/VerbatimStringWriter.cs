using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class VerbatimStringWriter : StringWriter
    {
        private readonly IOptions options;

        public VerbatimStringWriter(IOptions options)
            : base(options)
        {
            this.options = options;
        }

        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            VerbatimStringTemplate template = (VerbatimStringTemplate)fragment;
            output.Add("@")
                  .Add(this.options.Formatting.Quote)
                  .Add(template.Value, true)
                  .Add(this.options.Formatting.Quote);
        }
    }
}

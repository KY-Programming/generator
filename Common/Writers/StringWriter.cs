using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class StringWriter : ITemplateWriter
    {
        private readonly IOptions options;

        public StringWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            StringTemplate template = (StringTemplate)fragment;
            output.Add(this.options.Formatting.Quote)
                  .Add(template.Value)
                  .Add(this.options.Formatting.Quote);
        }
    }
}

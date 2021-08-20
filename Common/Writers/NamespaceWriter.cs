using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NamespaceWriter : ITemplateWriter
    {
        public BaseLanguage Language { get; }

        public NamespaceWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NamespaceTemplate template = (NamespaceTemplate)fragment;
            if (template.Children.Count == 0)
            {
                return;
            }

            bool hasNamespace = !string.IsNullOrEmpty(template.Name);
            if (hasNamespace)
            {
                output.Add($"{this.Language.NamespaceKeyword} {template.Name}")
                      .StartBlock();
            }
            output.Add(template.Children);
            if (hasNamespace)
            {
                output.EndBlock();
            }
        }
    }
}
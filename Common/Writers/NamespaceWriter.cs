using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NamespaceWriter : ITemplateWriter
    {
        protected string NamespaceKeyword { get; set; } = "namespace";

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
                output.Add($"${this.NamespaceKeyword} {template.Name}")
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

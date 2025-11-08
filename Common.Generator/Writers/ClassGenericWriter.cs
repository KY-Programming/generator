using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    internal class ClassGenericWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ClassGenericTemplate template = (ClassGenericTemplate)fragment;
            if (template.Constraints.Count == 0)
            {
                return;
            }
            output.Add(template.Name).BreakLine();
        }
    }
}
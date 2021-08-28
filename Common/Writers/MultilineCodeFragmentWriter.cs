using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class MultilineCodeFragmentWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            MultilineCodeFragment template = (MultilineCodeFragment)fragment;
            foreach (ICodeFragment codeFragment in template.Fragments)
            {
                output.Add(codeFragment);
            }
        }
    }
}

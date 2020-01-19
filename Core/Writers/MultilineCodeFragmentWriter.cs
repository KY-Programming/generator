using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class MultilineCodeFragmentWriter : ITemplateWriter
    {
        public ILanguage Language { get; }

        public MultilineCodeFragmentWriter(ILanguage language)
        {
            this.Language = language;
        }

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
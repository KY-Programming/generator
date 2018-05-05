using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class MultilineCodeFragmentWriter : ITemplateWriter
    {
        public ILanguage Language { get; }

        public MultilineCodeFragmentWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment codeFragment)
        {
            MultilineCodeFragment fragment = (MultilineCodeFragment)codeFragment;
            foreach (CodeFragment innerFragment in fragment.Fragments)
            {
                elements.Add(innerFragment, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment codeFragment)
        {
            MultilineCodeFragment fragment = (MultilineCodeFragment)codeFragment;
            foreach (CodeFragment innerFragment in fragment.Fragments)
            {
                fragments.Add(innerFragment, this.Language);
                fragments.AddNewLine();
            }
        }
    }
}
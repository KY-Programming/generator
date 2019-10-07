using KY.Generator.Languages;
using KY.Generator.Output;
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

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            MultilineCodeFragment template = (MultilineCodeFragment)fragment;
            foreach (ICodeFragment codeFragment in template.Fragments)
            {
                output.Add(codeFragment);
            }
        }

        //public virtual void Write(IMetaElementList elements, ICodeFragment codeFragment)
        //{
        //    MultilineCodeFragment fragment = (MultilineCodeFragment)codeFragment;
        //    foreach (ICodeFragment innerFragment in fragment.Fragments)
        //    {
        //        elements.Add(innerFragment, this.Language);
        //    }
        //}

        //public virtual void Write(IMetaFragmentList fragments, ICodeFragment codeFragment)
        //{
        //    MultilineCodeFragment fragment = (MultilineCodeFragment)codeFragment;
        //    foreach (ICodeFragment innerFragment in fragment.Fragments)
        //    {
        //        fragments.Add(innerFragment, this.Language);
        //        fragments.AddNewLine();
        //    }
        //}
    }
}
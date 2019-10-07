using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NullWriter : ITemplateWriter
    {

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
        }
        //public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        //{
        //    this.Write(elements.AddClosed().Code, fragment);
        //}

        //public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        //{
        //    fragments.Add("null");
        //}
    }
}
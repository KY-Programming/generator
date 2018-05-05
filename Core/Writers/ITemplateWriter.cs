using KY.Core.Meta;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public interface ITemplateWriter
    { 
        void Write(IMetaElementList elements, CodeFragment fragment);
        void Write(IMetaFragmentList fragments, CodeFragment fragment);
    }
}
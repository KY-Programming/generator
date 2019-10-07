using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public interface ITemplateWriter
    {
        void Write(ICodeFragment fragment, IOutputCache output);
    }
}
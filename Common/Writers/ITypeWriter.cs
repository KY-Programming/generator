using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public interface ITypeWriter
    {
        void Write(TypeTemplate template, IOutputCache output);
    }
}
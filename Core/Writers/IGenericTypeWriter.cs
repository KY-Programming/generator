using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public interface IGenericTypeWriter
    {
        void Write(GenericTypeTemplate template, IOutputCache output);
    }
}
using KY.Generator.Templates;

namespace KY.Generator.Output
{
    public interface IOutputCache : IOutputCacheBase
    {
        IOutputCache If(bool condition);
        IOutputCache EndIf();
    }
}
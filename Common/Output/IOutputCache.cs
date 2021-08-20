namespace KY.Generator.Output
{
    public interface IOutputCache : IOutputCacheBase
    {
        IOutputCache If(bool condition);
        IOutputCache EndIf();
        IOutputCache ExtraIndent(int indents = 1);
    }
}
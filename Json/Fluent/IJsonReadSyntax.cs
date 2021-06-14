using KY.Generator.Syntax;

namespace KY.Generator
{
    public interface IJsonReadSyntax
    {
        ISwitchToWriteSyntax FromFile(string relativePath);
    }
}
namespace KY.Generator;

public interface ITypeScriptSyntax : IFluentSyntax
{
    void NonStrict(bool value = true);
    void NoIndex();
}

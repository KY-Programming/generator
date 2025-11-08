namespace KY.Generator;

public interface ITypeScriptSyntax : IFluentSyntax
{
    void Strict(bool value = true);
    void NoIndex();
}

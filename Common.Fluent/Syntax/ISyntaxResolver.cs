namespace KY.Generator;

public interface ISyntaxResolver
{
    T Create<T>();
    void Register<TInterface, TSyntax>() where TInterface : IFluentSyntax where TSyntax : TInterface;
}

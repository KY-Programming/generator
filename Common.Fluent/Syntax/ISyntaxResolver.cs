using KY.Core.Dependency;

namespace KY.Generator;

public interface ISyntaxResolver
{
    /// <summary>
    /// Creates a syntax on its own command scope. Use it for syntaxes that only collect commands.
    /// </summary>
    T Create<T>();

    /// <summary>
    /// Creates a syntax on the scope of the given resolver. Use it for syntaxes that change the
    /// <see cref="Options"/> of the running fluent syntax - a new scope would come with its own options and the
    /// changes would be lost.
    /// </summary>
    T Create<T>(IDependencyResolver resolver);
    void Register<TInterface, TSyntax>() where TInterface : IFluentSyntax where TSyntax : TInterface;
}

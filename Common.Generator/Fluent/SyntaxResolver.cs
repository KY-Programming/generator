using KY.Core.Dependency;

namespace KY.Generator;

public class SyntaxResolver : ISyntaxResolver
{
    private readonly IDependencyResolver resolver;
    private readonly Dictionary<Type, Func<IDependencyResolver, object>> register = new();

    public SyntaxResolver(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public T Create<T>()
    {
        if (!this.register.TryGetValue(typeof(T), out Func<IDependencyResolver, object>? factory))
        {
            throw new InvalidOperationException($"Syntax for {typeof(T)} is not registered.");
        }
        IDependencyResolver commandResolver = this.resolver.CloneForCommand();
        return (T)factory(commandResolver);
    }

    public void Register<TInterface, TSyntax>() where TInterface : IFluentSyntax where TSyntax : TInterface
    {
        this.register.Add(typeof(TInterface), x => x.Create<TSyntax>());
    }
}

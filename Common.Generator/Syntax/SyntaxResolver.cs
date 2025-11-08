using KY.Core;
using KY.Core.Dependency;

namespace KY.Generator.Syntax;

public class SyntaxResolver : ISyntaxResolver
{
    private readonly IDependencyResolver resolver;

    public SyntaxResolver(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public T Get<T>()
    {
        IDependencyResolver commandResolver = this.resolver.CloneForCommand();
        return commandResolver.Create<FluentSyntax>().CastTo<T>();
    }
}

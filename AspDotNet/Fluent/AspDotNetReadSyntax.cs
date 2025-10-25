using KY.Core.Dependency;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent;

internal class AspDotNetReadSyntax : ExecutableSyntax, IAspDotNetReadSyntax
{
    private readonly IDependencyResolver resolver;

    public AspDotNetReadSyntax(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public IAspDotNetReadSyntax FromController<T>()
    {
        Type type = typeof(T);
        this.Commands.Add(new LoadCommandParameters
        {
            Assembly = type.Assembly.Location
        });
        this.Commands.Add(new AspDotNetReadControllerCommandParameters
        {
            Namespace = type.Namespace,
            Name = type.Name
        });
        return this;
    }

    public IAspDotNetReadSyntax FromHub<T>()
    {
        Type type = typeof(T);
        this.Commands.Add(new LoadCommandParameters
        {
            Assembly = type.Assembly.Location
        });
        this.Commands.Add(new AspDotNetReadHubCommandParameters
        {
            Namespace = type.Namespace,
            Name = type.Name
        });
        return this;
    }
}

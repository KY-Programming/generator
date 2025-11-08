using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Commands;

namespace KY.Generator;

internal class AspDotNetReadSyntax : IExecutableSyntax, IAspDotNetReadSyntax
{
    private readonly IDependencyResolver resolver;
    public List<GeneratorCommandParameters> Commands { get; } = [];

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

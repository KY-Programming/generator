using KY.Core.Dependency;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent;

internal class AspDotNetReadSyntax : IAspDotNetReadSyntax, IExecutableSyntax
{
    private readonly IDependencyResolver resolver;

    public List<IGeneratorCommand> Commands { get; } = new();

    public AspDotNetReadSyntax(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public IAspDotNetReadSyntax FromController<T>()
    {
        Type type = typeof(T);
        LoadCommand loadCommand = this.resolver.Create<LoadCommand>();
        loadCommand.Parameters.Assembly = type.Assembly.Location;
        this.Commands.Add(loadCommand);
        AspDotNetReadControllerCommand command = this.resolver.Create<AspDotNetReadControllerCommand>();
        command.Parameters.Namespace = type.Namespace;
        command.Parameters.Name = type.Name;
        this.Commands.Add(command);
        return this;
    }

    public IAspDotNetReadSyntax FromHub<T>()
    {
        Type type = typeof(T);
        LoadCommand loadCommand = this.resolver.Create<LoadCommand>();
        loadCommand.Parameters.Assembly = type.Assembly.Location;
        this.Commands.Add(loadCommand);
        AspDotNetReadHubCommand command = this.resolver.Create<AspDotNetReadHubCommand>();
        command.Parameters.Namespace = type.Namespace;
        command.Parameters.Name = type.Name;
        this.Commands.Add(command);
        return this;
    }
}

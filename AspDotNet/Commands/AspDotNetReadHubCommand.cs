using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;

namespace KY.Generator.AspDotNet.Commands;

internal class AspDotNetReadHubCommand : GeneratorCommand<AspDotNetReadHubCommandParameters>
{
    private readonly IDependencyResolver resolver;
    public static string[] Names { get; } = [ToCommand(nameof(AspDotNetReadHubCommand)), "asp-read-hub"];

    public AspDotNetReadHubCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);

        AspDotNetReadConfiguration readConfiguration = new();
        readConfiguration.Hub = new AspDotNetReadHubConfiguration();
        readConfiguration.Hub.Namespace = this.Parameters.Namespace;
        readConfiguration.Hub.Name = this.Parameters.Name;

        this.resolver.Create<AspDotNetHubReader>().Read(readConfiguration);

        return this.Success();
    }
}

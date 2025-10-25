using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;

namespace KY.Generator.AspDotNet.Commands;

internal class AspDotNetReadControllerCommand : GeneratorCommand<AspDotNetReadControllerCommandParameters>
{
    private readonly IDependencyResolver resolver;
    public static string[] Names { get; } = [..ToCommand(nameof(AspDotNetReadControllerCommand)), "asp-read-controller"];

    public AspDotNetReadControllerCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);

        AspDotNetReadConfiguration readConfiguration = new();
        readConfiguration.Controller = new AspDotNetReadControllerConfiguration();
        readConfiguration.Controller.Namespace = this.Parameters.Namespace;
        readConfiguration.Controller.Name = this.Parameters.Name;

        this.resolver.Create<AspDotNetControllerReader>().Read(readConfiguration);

        return this.Success();
    }
}

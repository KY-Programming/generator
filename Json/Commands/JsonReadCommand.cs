using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Readers;
using KY.Generator.Output;

namespace KY.Generator.Json.Commands;

public class JsonReadCommand : GeneratorCommand<JsonReadCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public static string[] Names { get; } = [..ToCommand(nameof(JsonReadCommand)), "json-read"];

    public JsonReadCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        JsonReadConfiguration configuration = new();
        configuration.Source = this.Parameters.RelativePath;
        configuration.BasePath = (this.resolver.Get<IOutput>() as FileOutput)?.BasePath;

        this.resolver.Create<JsonReader>().Read(configuration);

        return this.Success();
    }
}

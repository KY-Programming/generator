using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Readers;
using KY.Generator.Output;

namespace KY.Generator.Json.Commands
{
    public class JsonReadCommand : GeneratorCommand<JsonReadCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "json-read" };

        public JsonReadCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            JsonReadConfiguration configuration = new JsonReadConfiguration();
            configuration.Source = this.Parameters.RelativePath;
            configuration.BasePath = (output as FileOutput)?.BasePath;

            this.resolver.Create<JsonReader>().Read(configuration, this.TransferObjects);

            return this.Success();
        }
    }
}
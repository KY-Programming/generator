using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Writers;
using KY.Generator.Output;

namespace KY.Generator.Json.Commands
{
    public class JsonWriteCommand : GeneratorCommand<JsonWriteCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "json-write" };

        public JsonWriteCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            JsonWriteConfiguration configuration = new JsonWriteConfiguration();
            configuration.Language = CsharpLanguage.Instance;
            configuration.FormatNames = this.Parameters.FormatNames;
            configuration.RelativePath = this.Parameters.ModelPath;
            configuration.FormatNames = this.Parameters.FormatNames;
            configuration.FieldsToProperties = this.Parameters.FieldsToProperties;
            configuration.Name = this.Parameters.ModelName;
            configuration.Namespace = this.Parameters.ModelNamespace;
            configuration.PropertiesToFields = this.Parameters.PropertiesToFields;
            configuration.SkipNamespace = this.Parameters.SkipNamespace;
            configuration.WithReader = this.Parameters.WithReader;

            this.resolver.Create<JsonWriter>().Write(configuration, this.TransferObjects, output);

            return this.Success();
        }
    }
}
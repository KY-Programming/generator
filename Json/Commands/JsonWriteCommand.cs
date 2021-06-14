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
            configuration.Object = new JsonWriteObjectConfiguration();
            configuration.Object.RelativePath = this.Parameters.ModelPath;
            configuration.Object.FormatNames = this.Parameters.FormatNames;
            configuration.Object.FieldsToProperties = this.Parameters.FieldsToProperties;
            configuration.Object.Name = this.Parameters.ModelName;
            configuration.Object.Namespace = this.Parameters.ModelNamespace;
            configuration.Object.PropertiesToFields = this.Parameters.PropertiesToFields;
            configuration.Object.SkipNamespace = this.Parameters.SkipNamespace;
            configuration.Object.WithReader = this.Parameters.WithReader;
            if (this.Parameters.WithReader)
            {
                configuration.Reader = new JsonWriteReaderConfiguration();
                configuration.Reader.FormatNames = this.Parameters.FormatNames;
                configuration.Reader.RelativePath = this.Parameters.ReaderPath;
                configuration.Reader.Name = this.Parameters.ReaderName;
                configuration.Reader.Namespace = this.Parameters.ReaderNamespace;
            }

            this.resolver.Create<JsonWriter>().Write(configuration, this.TransferObjects, output);

            return this.Success();
        }
    }
}
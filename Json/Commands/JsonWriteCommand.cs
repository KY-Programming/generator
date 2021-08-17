using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Writers;
using KY.Generator.Languages.Extensions;
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
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.SetOutputId(this.TransferObjects);
            options.Language = CsharpLanguage.Instance;
            options.Formatting.FromLanguage(options.Language);

            this.resolver.Create<JsonWriter>().Write(this.TransferObjects, this.Parameters.RelativePath, output, this.Parameters.WithReader);

            return this.Success();
        }
    }
}

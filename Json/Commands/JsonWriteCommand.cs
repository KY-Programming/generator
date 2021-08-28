using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Writers;

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

        public override IGeneratorCommandResult Run()
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.Language = this.resolver.Get<CsharpLanguage>();

            this.resolver.Create<JsonWriter>().Write(this.Parameters.RelativePath, this.Parameters.WithReader);

            return this.Success();
        }
    }
}

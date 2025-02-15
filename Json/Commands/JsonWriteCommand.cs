using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Writers;
using KY.Generator.TypeScript;

namespace KY.Generator.Json.Commands;

public class JsonWriteCommand(IDependencyResolver resolver) : GeneratorCommand<JsonWriteCommandParameters>
{
    public static string[] Names { get; } = [ToCommand(nameof(JsonWriteCommand)), "json-write"];

    public override void Prepare()
    {
        Options options = resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = resolver.Get<CsharpLanguage>();
        TypeScriptOptions typeScriptOptions = options.Get<TypeScriptOptions>();
        typeScriptOptions.SetStrict(this.Parameters.RelativePath, resolver);
    }

    public override IGeneratorCommandResult Run()
    {
        resolver.Create<JsonWriter>().FormatNames().Write(this.Parameters.RelativePath, this.Parameters.WithReader);

        return this.Success();
    }
}

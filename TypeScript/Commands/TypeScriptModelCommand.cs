using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.TypeScript.Commands;

public class TypeScriptModelCommand(IDependencyResolver resolver) : GeneratorCommand<GeneratorCommandParameters>
{
    public static string[] Names { get; } = [..ToCommand(nameof(TypeScriptModelCommand)), "typescript-model"];

    public override void Prepare()
    {
        Options options = resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = resolver.Get<TypeScriptLanguage>();
        generatorOptions.SkipNamespace = true;
        generatorOptions.PropertiesToFields = true;
    }

    public override IGeneratorCommandResult Run()
    {
        // TODO: Fix path is null
        resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        resolver.Create<TypeScriptModelWriter>().FormatNames().Write(this.Parameters.RelativePath);
        resolver.Create<TypeScriptIndexHelper>().Execute(this.Parameters.RelativePath);
        return this.Success();
    }
}

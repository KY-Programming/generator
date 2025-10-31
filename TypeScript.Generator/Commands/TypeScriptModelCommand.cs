using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.TypeScript.Commands;

internal class TypeScriptModelCommand(IDependencyResolver resolver) : GeneratorCommand<TypeScriptModelCommandParameters>
{
    public override void Prepare()
    {
        Options options = resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = resolver.Get<TypeScriptLanguage>();
        generatorOptions.SkipNamespace = true;
        generatorOptions.PropertiesToFields = true;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        // TODO: Fix path is null
        resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        resolver.Create<TypeScriptModelWriter>().FormatNames().Write();
        resolver.Create<TypeScriptIndexHelper>().Execute(this.Parameters.RelativePath);
        return this.SuccessAsync();
    }
}

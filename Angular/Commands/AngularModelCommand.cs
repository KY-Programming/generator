using KY.Core.Dependency;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Commands;

public class AngularModelCommand(IDependencyResolver resolver) : GeneratorCommand<GeneratorCommandParameters>
{
    public static string[] Names { get; } = [ToCommand(nameof(AngularModelCommand)), "angular-model"];

    public override void Prepare()
    {
        Options options = resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = resolver.Get<AngularTypeScriptLanguage>();
        generatorOptions.Formatting.AllowedSpecialCharacters = "$";
        generatorOptions.SkipNamespace = true;
        generatorOptions.PropertiesToFields = true;
        TypeScriptOptions typeScriptOptions = options.Get<TypeScriptOptions>();
        // TODO: Fix path is null
        typeScriptOptions.SetStrict(this.Parameters.RelativePath, resolver);
    }

    public override IGeneratorCommandResult Run()
    {
        // TODO: Fix path is null
        resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        resolver.Create<AngularModelWriter>().FormatNames().Write(this.Parameters.RelativePath);
        resolver.Create<TypeScriptIndexHelper>().Execute(this.Parameters.RelativePath);
        return this.Success();
    }
}
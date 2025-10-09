using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands;

internal class ReflectionCommand(IDependencyResolver resolver) : GeneratorCommand<ReflectionCommandParameters>
{
    public static string[] Names { get; } = [ToCommand(nameof(ReflectionCommand)), "reflection"];

    public override IGeneratorCommandResult Run()
    {
        Options options = resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false
                                        ? resolver.Get<CsharpLanguage>()
                                        : resolver.Get<TypeScriptLanguage>();
        if (generatorOptions.Language.IsTypeScript())
        {
            generatorOptions.SkipNamespace = true;
            generatorOptions.PropertiesToFields = true;
        }
        generatorOptions.SetFromParameter(this.Parameters);

        ReflectionReadConfiguration readConfiguration = new();
        readConfiguration.Name = this.Parameters.Name;
        readConfiguration.Namespace = this.Parameters.Namespace;
        readConfiguration.Assembly = this.Parameters.Assembly;
        readConfiguration.OnlySubTypes = this.Parameters.OnlySubTypes;

        resolver.Create<ReflectionReader>().Read(readConfiguration, generatorOptions);
        resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        ReflectionWriter writer = resolver.Create<ReflectionWriter>();
        writer.FormatNames();
        writer.Write(this.Parameters.RelativePath);

        resolver.Get<IEnvironment>().TransferObjects.AddIfNotExists(resolver.Get<List<ITransferObject>>());

        return this.Success();
    }
}

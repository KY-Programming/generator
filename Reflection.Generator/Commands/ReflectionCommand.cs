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

internal class ReflectionCommand : GeneratorCommand<ReflectionCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public ReflectionCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false
            ? this.resolver.Get<CsharpLanguage>()
            : this.resolver.Get<TypeScriptLanguage>();
        if (generatorOptions.Language.IsTypeScript())
        {
            generatorOptions.SkipNamespace = true;
            generatorOptions.PropertiesToFields = true;
        }
        generatorOptions.SetFromParameter(this.Parameters);

        ReflectionReadConfiguration readConfiguration = new();
        readConfiguration.Name = this.Parameters.Name;
        readConfiguration.Namespace = this.Parameters.Namespace;
        readConfiguration.OnlySubTypes = this.Parameters.OnlySubTypes;

        this.resolver.Create<ReflectionReader>().Read(readConfiguration, generatorOptions);
        this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        ReflectionWriter writer = this.resolver.Create<ReflectionWriter>();
        writer.FormatNames();
        writer.Write(this.Parameters.RelativePath);

        this.resolver.Get<IEnvironment>().TransferObjects.AddIfNotExists(this.resolver.Get<List<ITransferObject>>());
        return this.Success();
    }
}

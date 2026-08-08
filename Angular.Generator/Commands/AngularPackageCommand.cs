using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript;

namespace KY.Generator.Angular.Commands;

internal class AngularPackageCommand : GeneratorCommand<AngularPackageCommandParameters>
{
    private readonly IDependencyResolver resolver;
    private readonly IOutput output;
    private string nameWithoutScope;
    private string packagePath;
    private string relativePackagePath;
    private string? servicePath;
    private string? modelPath;
    private readonly List<IGeneratorCommand> subCommands = [];

    public AngularPackageCommand(IDependencyResolver resolver, IOutput output)
    {
        this.resolver = resolver;
        this.output = output;
    }

    public override void Prepare()
    {
        this.nameWithoutScope = this.Parameters.Name.Split('/').Last();
        this.relativePackagePath = FileSystem.Combine(this.Parameters.RelativePath, AngularPackageWriter.BasePackageName, "projects", this.nameWithoutScope);
        this.packagePath = FileSystem.Combine(this.output.ToString(), this.relativePackagePath);
        base.Prepare();

        // The paths configured on the sub commands are relative to the library source folder of the generated npm
        // package (that is what the user writes in the fluent api), the commands themselves expect a path relative
        // to the output root. Both variants are needed: the relative one for the public-api.ts of the package, the
        // absolute one for the sub commands.
        this.servicePath = this.Parameters.SubCommands.OfType<AngularServiceCommandParameters>().FirstOrDefault()?.RelativePath;
        this.modelPath = this.Parameters.SubCommands.OfType<AngularModelCommandParameters>().FirstOrDefault()?.RelativePath;
        string libPath = FileSystem.Combine(this.relativePackagePath, "src", "lib");
        foreach (GeneratorCommandParameters subCommand in this.Parameters.SubCommands)
        {
            subCommand.RelativePath = FileSystem.Combine(libPath, subCommand.RelativePath ?? string.Empty);
        }
        foreach (AngularServiceCommandParameters subCommand in this.Parameters.SubCommands.OfType<AngularServiceCommandParameters>())
        {
            subCommand.RelativeModelPath = FileSystem.Combine(libPath, subCommand.RelativeModelPath ?? string.Empty);
        }
    }

    public override async Task<IGeneratorCommandResult> Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = this.resolver.Get<AngularTypeScriptLanguage>();
        generatorOptions.SkipNamespace = true;
        TypeScriptOptions typeScriptOptions = options.Get<TypeScriptOptions>();
        typeScriptOptions.SetStrictFromConfig(this.Parameters.RelativePath, this.resolver);
        typeScriptOptions.ForceIndex = true;

        AngularPackageWriter writer = this.resolver.Create<AngularPackageWriter>();
        writer.Write(this.nameWithoutScope, this.Parameters.Name, this.Parameters.Version, this.packagePath, this.Parameters.DependsOn, this.Parameters.CliVersion, this.servicePath, this.modelPath, this.Parameters.IncrementVersion, this.Parameters.VersionFromNpm);

        GeneratorCommandRunner runner = this.resolver.Create<GeneratorCommandRunner>();
        this.subCommands.AddRange(runner.Create(this.Parameters.SubCommands, this.resolver));
        return await runner.Run(this.subCommands);
    }

    public override void FollowUp()
    {
        base.FollowUp();
        this.subCommands.ForEach(command => command.FollowUp());
        bool publish = this.Parameters.Publish || this.Parameters.PublishLocal;
        if (!this.Parameters.Build && !publish)
        {
            return;
        }
        AngularPackageBuilder builder = this.resolver.Create<AngularPackageBuilder>();
        builder.Build(this.packagePath);
        if (this.Parameters.Publish)
        {
            builder.Publish(this.packagePath);
        }
        if (this.Parameters.PublishLocal)
        {
            builder.PublishLocal(this.packagePath);
        }
    }
}

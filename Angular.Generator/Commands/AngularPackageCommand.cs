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
        // TODO: Reimplement
        // this.Commands.ForEach(command => command.Prepare());
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        Options options = this.resolver.Get<Options>();
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        generatorOptions.SetFromParameter(this.Parameters);
        generatorOptions.Language = this.resolver.Get<AngularTypeScriptLanguage>();
        generatorOptions.SkipNamespace = true;
        generatorOptions.PropertiesToFields = true;
        TypeScriptOptions typeScriptOptions = options.Get<TypeScriptOptions>();
        typeScriptOptions.SetStrict(this.Parameters.RelativePath, this.resolver);
        typeScriptOptions.ForceIndex = true;

        string? servicePath = this.Parameters.SubCommands.OfType<AngularServiceCommandParameters>().FirstOrDefault()?.RelativePath;
        string? modelPath = this.Parameters.SubCommands.OfType<AngularModelCommandParameters>().FirstOrDefault()?.RelativePath;
        string libPath = FileSystem.Combine(this.relativePackagePath, "src", "lib");
        // TODO: Reimplement
        // this.Commands.ForEach(command => command.Parameters.RelativePath = FileSystem.Combine(libPath, command.Parameters.RelativePath));
        // this.Commands.OfType<AngularServiceCommand>().ForEach(command => command.Parameters.RelativeModelPath = FileSystem.Combine(libPath, command.Parameters.RelativeModelPath));

        AngularPackageWriter writer = this.resolver.Create<AngularPackageWriter>();
        writer.Write(this.nameWithoutScope, this.Parameters.Name, this.Parameters.Version, this.packagePath, this.Parameters.DependsOn, this.Parameters.CliVersion, servicePath, modelPath, this.Parameters.IncrementVersion, this.Parameters.VersionFromNpm);
        // TODO: Reimplement
        // this.Commands.ForEach(command => command.Run());
        return this.SuccessAsync();
    }

    public override void FollowUp()
    {
        base.FollowUp();
        // TODO: Reimplement
        // this.Commands.ForEach(command => command.FollowUp());
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

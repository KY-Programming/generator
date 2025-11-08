using KY.Generator.Command;

namespace KY.Generator.Angular;

internal class AngularPackageSyntax : IExecutableSyntax, IAngularPackageSyntax
{
    private readonly AngularPackageCommandParameters command;
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public AngularPackageSyntax(AngularPackageCommandParameters command)
    {
        this.command = command;
    }

    public IAngularPackageSyntax Name(string packageName)
    {
        this.command.Name = packageName;
        return this;
    }

    public IAngularPackageSyntax Version(string version)
    {
        this.command.Version = version;
        return this;
    }

    public IAngularPackageSyntax VersionFromNpm()
    {
        this.command.VersionFromNpm = true;
        return this;
    }

    public IAngularPackageSyntax IncrementMajorVersion()
    {
        this.command.IncrementVersion = IncrementVersion.Major;
        return this;
    }

    public IAngularPackageSyntax IncrementMinorVersion()
    {
        this.command.IncrementVersion = IncrementVersion.Minor;
        return this;
    }

    public IAngularPackageSyntax IncrementPatchVersion()
    {
        this.command.IncrementVersion = IncrementVersion.Patch;
        return this;
    }

    public IAngularPackageSyntax CliVersion(string version)
    {
        this.command.CliVersion = version;
        return this;
    }

    public IAngularPackageSyntax DependsOn(string packageName, string version)
    {
        this.command.DependsOn.Add(new AngularPackageDependsOnParameter(packageName, version));
        return this;
    }

    public IAngularPackageSyntax OutputPath(string path)
    {
        this.command.RelativePath = path;
        return this;
    }

    public IAngularPackageSyntax Build()
    {
        this.command.Build = true;
        return this;
    }

    public IAngularPackageSyntax Publish()
    {
        this.command.Publish = true;
        return this;
    }

    public IAngularPackageSyntax PublishLocal()
    {
        this.command.PublishLocal = true;
        return this;
    }

    public IAngularPackageSyntax Models(Action<IAngularModelSyntax>? action = null)
    {
        AngularModelCommandParameters modelCommand = new();
        this.Commands.Add(modelCommand);
        action?.Invoke(new AngularModelSyntax(this, modelCommand));
        return this;
    }

    public IAngularPackageSyntax Services(Action<IAngularServiceSyntax>? action = null)
    {
        AngularServiceCommandParameters serviceCommand = new();
        this.Commands.Add(serviceCommand);
        action?.Invoke(new AngularServiceSyntax(this, serviceCommand));
        return this;
    }
}

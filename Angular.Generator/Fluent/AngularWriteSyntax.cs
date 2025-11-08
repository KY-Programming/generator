using KY.Core;
using KY.Generator.Command;

namespace KY.Generator.Angular;

internal class AngularWriteSyntax : IExecutableSyntax, IAngularWriteSyntax
{
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public IAngularWriteSyntax Models(Action<IAngularModelSyntax>? action = null)
    {
        AngularModelCommandParameters command = new();
        this.Commands.Add(command);
        action?.Invoke(new AngularModelSyntax(this, command));
        return this;
    }

    public IAngularWriteSyntax Services(Action<IAngularServiceSyntax>? action = null)
    {
        AngularServiceCommandParameters command = new();
        this.Commands.Add(command);
        action?.Invoke(new AngularServiceSyntax(this, command));
        return this;
    }

    public IAngularWriteSyntax Package(Action<IAngularPackageSyntax> action)
    {
        action.AssertIsNotNull();
        AngularPackageCommandParameters command = new();
        this.Commands.Add(command);
        action.Invoke(new AngularPackageSyntax(command));
        return this;
    }
}

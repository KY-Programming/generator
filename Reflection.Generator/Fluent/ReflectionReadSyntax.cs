using KY.Generator.Command;
using KY.Generator.Commands;

namespace KY.Generator.Reflection.Fluent;

internal class ReflectionReadSyntax : IExecutableSyntax, IReflectionReadSyntax
{
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public IReflectionReadSyntax FromType<T>(Action<IReflectionFromTypeSyntax>? action = null)
    {
        Type type = typeof(T);
        this.Commands.Add(new LoadCommandParameters
        {
            Assembly = type.Assembly.Location
        });
        ReflectionReadCommandParameters command = new()
        {
            Namespace = type.Namespace,
            Name = type.Name
        };
        this.Commands.Add(command);
        action?.Invoke(new ReflectionFromTypeSyntax(command));
        return this;
    }
}

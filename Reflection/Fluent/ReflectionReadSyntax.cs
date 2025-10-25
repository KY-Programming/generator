using KY.Generator.Commands;
using KY.Generator.Syntax;

namespace KY.Generator;

internal class ReflectionReadSyntax : ExecutableSyntax, IReflectionReadSyntax
{
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

using KY.Generator.Reflection.Commands;

namespace KY.Generator.Reflection.Fluent;

internal class ReflectionFromTypeSyntax : IReflectionFromTypeSyntax
{
    private readonly ReflectionReadCommandParameters command;

    public ReflectionFromTypeSyntax(ReflectionReadCommandParameters command)
    {
        this.command = command;
    }

    public IReflectionFromTypeSyntax OnlySubTypes()
    {
        this.command.OnlySubTypes = true;
        return this;
    }
}

using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent;

internal class ReflectionReadSyntax : IReflectionReadSyntax, IExecutableSyntax
{
    private readonly IReadFluentSyntaxInternal syntax;

    public List<IGeneratorCommand> Commands { get; } = new();

    public ReflectionReadSyntax(IReadFluentSyntaxInternal syntax)
    {
        this.syntax = syntax;
    }

    public IReflectionReadSyntax FromType<T>(Action<IReflectionFromTypeSyntax>? action = null)
    {
        Type type = typeof(T);
        LoadCommand loadCommand = this.syntax.Resolver.Create<LoadCommand>();
        loadCommand.Parameters.Assembly = type.Assembly.Location;
        this.Commands.Add(loadCommand);
        ReflectionReadCommand command = this.syntax.Resolver.Create<ReflectionReadCommand>();
        command.Parameters.Namespace = type.Namespace;
        command.Parameters.Name = type.Name;
        this.Commands.Add(command);
        action?.Invoke(new ReflectionFromTypeSyntax(command));
        return this;
    }
}

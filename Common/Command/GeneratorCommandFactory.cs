using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Models;

namespace KY.Generator.Command;

public class GeneratorCommandFactory(IDependencyResolver resolverFallback)
{
    private readonly Dictionary<string, Type> commands = new();
    private readonly List<Type> prepareCommands = [];

    public GeneratorCommandFactory Register<T>(IEnumerable<string> names) where T : IGeneratorCommand
    {
        this.Register(typeof(T), names.Unique());
        if (typeof(IPrepareCommand).IsAssignableFrom(typeof(T)))
        {
            this.prepareCommands.Add(typeof(T));
        }
        return this;
    }

    public GeneratorCommandFactory Register(Type command, IEnumerable<string> names)
    {
        foreach (string name in names)
        {
            if (this.commands.ContainsKey(name))
            {
                throw new InvalidOperationException($"Command '{name}' is already registered.");
            }
            this.commands.Add(name, command);
        }
        return this;
    }

    public List<IGeneratorCommand> Create(params string[] parameters)
    {
        return this.Create(CliCommandReader.Read(parameters));
    }

    public List<IGeneratorCommand> Create(IEnumerable<CliCommand> cliCommands, IDependencyResolver? resolver = null)
    {
        bool allCommandsFound = true;
        List<IGeneratorCommand> foundCommands = [];
        foreach (CliCommand cliCommand in cliCommands)
        {
            if (!this.commands.TryGetValue(cliCommand.Name, out Type type))
            {
                allCommandsFound = false;
                GeneratorErrors.CommandNotFoundError(cliCommand);
                continue;
            }
            IGeneratorCommand command = this.Create(type, cliCommand.Parameters, resolver);
            foundCommands.Add(command);
        }
        if (!allCommandsFound)
        {
            GeneratorErrors.CommandDocumentationHint();
        }
        return foundCommands;
    }

    private IGeneratorCommand Create(Type type, IEnumerable<CliCommandParameter>? parameters = null, IDependencyResolver? resolver = null)
    {
        resolver ??= resolverFallback.CloneForCommand();
        List<CliCommandParameter> globalParameters = resolver.Get<IEnvironment>().Parameters;
        parameters ??= [];
        IGeneratorCommand command = (IGeneratorCommand)resolver.Create(type);
        command.OriginalParameters = parameters.Concat(globalParameters).ToList();
        return command;
    }

    public IEnumerable<IPrepareCommand> CreatePrepareCommands()
    {
        return this.prepareCommands.Select(type => (IPrepareCommand)this.Create(type));
    }
}

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
            string lowerName = name.ToLowerInvariant();
            if (this.commands.ContainsKey(lowerName))
            {
                throw new InvalidOperationException($"Command '{name}' is already registered.");
            }
            this.commands.Add(lowerName, command);
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
            if (!this.commands.TryGetValue(cliCommand.Name.ToLowerInvariant(), out Type type))
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

    public List<IGeneratorCommand> Create(IEnumerable<GeneratorCommandParameters> parametersList, IDependencyResolver? resolver = null)
    {
        bool allCommandsFound = true;
        List<IGeneratorCommand> foundCommands = [];
        foreach (GeneratorCommandParameters parameters in parametersList)
        {
            if (!this.commands.TryGetValue(parameters.CommandName.ToLowerInvariant(), out Type type))
            {
                allCommandsFound = false;
                GeneratorErrors.CommandNotFoundError(parameters.CommandName);
                continue;
            }
            IGeneratorCommand command = this.Create(type, parameters, resolver);
            foundCommands.Add(command);
        }
        if (!allCommandsFound)
        {
            GeneratorErrors.CommandDocumentationHint();
        }
        return foundCommands;
    }

    private IGeneratorCommand Create(Type type, GeneratorCommandParameters parameters, IDependencyResolver? resolver = null)
    {
        resolver ??= resolverFallback.CloneForCommand();
        IGeneratorCommand command = (IGeneratorCommand)resolver.Create(type);
        command.Parameters.SetFrom(parameters);
        return command;
    }

    public IEnumerable<IPrepareCommand> CreatePrepareCommands()
    {
        return this.prepareCommands.Select(type => (IPrepareCommand)this.Create(type));
    }
}

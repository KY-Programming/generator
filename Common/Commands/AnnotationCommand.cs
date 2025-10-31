using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Helpers;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator.Commands;

internal class AnnotationCommand : GeneratorCommand<AnnotationCommandParameters>
{
    private static readonly List<Assembly> processedAssemblies = [];
    private readonly IDependencyResolver resolver;
    private readonly IEnvironment environment;

    public AnnotationCommand(IDependencyResolver resolver, IEnvironment environment)
    {
        this.resolver = resolver;
        this.environment = environment;
    }

    public override async Task<IGeneratorCommandResult> Run()
    {
        if (this.environment.LoadedAssemblies.Count == 0)
        {
            if (!this.environment.IsBeforeBuild)
            {
                Logger.Warning($"Can not run '{this.Parameters.CommandName}' command without loaded assemblies. Add at least one 'load -assembly=<assembly-path>' command before.");
            }
            return this.Success();
        }
        foreach (Assembly assembly in this.environment.LoadedAssemblies.ToList())
        {
            if (processedAssemblies.Contains(assembly))
            {
                continue;
            }
            processedAssemblies.Add(assembly);
            if (!this.Parameters.IsOnlyAsync && assembly.IsAsync())
            {
                return this.SwitchAsync();
            }
            List<CliCommandParameter> globalParameters = this.Parameters.GetType().GetProperties()
                                                             .Where(x => x.GetCustomAttribute<GeneratorGlobalParameterAttribute>() != null)
                                                             .Select(x =>
                                                                 new CliCommandParameter(x.Name, x.GetMethod.Invoke(this.Parameters, null)?.ToString())
                                                             )
                                                             .Where(x => !string.IsNullOrEmpty(x.Value))
                                                             .ToList();
            List<Attribute> assemblyAttributes = assembly.GetCustomAttributes().ToList();
            if (assemblyAttributes.Count > 0)
            {
                List<CliCommand> rawCommands = [];
                foreach (IGeneratorCommandAttribute attribute in assemblyAttributes.OfType<IGeneratorCommandAttribute>())
                {
                    rawCommands.AddRange(this.CreateCommands(attribute, globalParameters, null, null, assembly.GetName().Name, assembly.Location));
                }
                this.ApplyAdditionalCommandParameters(rawCommands, assemblyAttributes.OfType<IGeneratorCommandAdditionalParameterAttribute>());
                IGeneratorCommandResult result = await this.ExecuteCommands(rawCommands);
                if (!result.Success)
                {
                    return result;
                }
            }
            foreach (Type objectType in TypeHelper.GetTypes(assembly))
            {
                List<Attribute> attributes = objectType.GetCustomAttributes().ToList();
                List<CliCommand> rawCommands = [];
                foreach (IGeneratorCommandAttribute attribute in attributes.OfType<IGeneratorCommandAttribute>())
                {
                    rawCommands.AddRange(this.CreateCommands(attribute, globalParameters, objectType.Name, objectType.Namespace, assembly.GetName().Name, assembly.Location));
                }
                this.ApplyAdditionalCommandParameters(rawCommands, attributes.OfType<IGeneratorCommandAdditionalParameterAttribute>());
                IGeneratorCommandResult result = await this.ExecuteCommands(rawCommands);
                if (!result.Success)
                {
                    return result;
                }
            }
        }
        return this.Success();
    }

    private IEnumerable<CliCommand> CreateCommands(IGeneratorCommandAttribute attribute, List<CliCommandParameter> globalParameters, string? typeName = null, string? nameSpace = null, string? assembly = null, string? assemblyLocation = null)
    {
        return attribute.Commands.Select(x =>
        {
            CliCommand command = new(x.Command);
            command.Parameters.AddRange(x.Parameters.Select(CliCommandParameter.Parse));
            command.Parameters.AddRange(globalParameters);
            foreach (CliCommandParameter parameter in command.Parameters)
            {
                parameter.Value = parameter.Value.Replace("$NAMESPACE$", nameSpace ?? string.Empty)
                                           .Replace("$NAME$", typeName ?? string.Empty)
                                           .Replace("$ASSEMBLY$", assembly ?? string.Empty)
                                           .Replace("$ASSEMBLYLOCATION$", assemblyLocation ?? string.Empty)
                                           .Replace("$ASSEMBLYPATH$", FileSystem.Parent(assemblyLocation ?? string.Empty) ?? string.Empty);
            }
            return command;
        });
    }

    private void ApplyAdditionalCommandParameters(List<CliCommand> rawCommands, IEnumerable<IGeneratorCommandAdditionalParameterAttribute> attributes)
    {
        foreach (IGeneratorCommandAdditionalParameterAttribute additionalParameterAttribute in attributes)
        {
            foreach (AttributeCommandConfiguration additionalParameters in additionalParameterAttribute.Commands)
            {
                foreach (CliCommand command in rawCommands.Where(x => x.Name == additionalParameters.Command || additionalParameters.Command == "*"))
                {
                    command.Parameters.AddRange(additionalParameters.Parameters.Select(CliCommandParameter.Parse));
                }
            }
        }
    }

    private async Task<IGeneratorCommandResult> ExecuteCommands(List<CliCommand> rawCommands)
    {
        if (rawCommands.Count == 0)
        {
            return this.Success();
        }
        IDependencyResolver commandResolver = this.resolver.CloneForCommand();
        GeneratorCommandRunner generatorCommandRunner = commandResolver.Get<GeneratorCommandRunner>();
        List<IGeneratorCommand> commands = commandResolver.Get<GeneratorCommandFactory>().Create(rawCommands, commandResolver);
        commands.ForEach(x => x.Parse());
        IGeneratorCommandResult commandResult = await generatorCommandRunner.Run(commands);
        if (!commandResult.Success)
        {
            return commandResult;
        }
        this.environment.TransferObjects.AddIfNotExists(commandResolver.Get<List<ITransferObject>>());
        return this.Success();
    }
}

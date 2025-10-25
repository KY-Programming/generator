using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Helpers;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator.Commands;

public class AnnotationCommand : GeneratorCommand<AnnotationCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public static string[] Names { get; } = [ToCommand(nameof(AnnotationCommand)), "annotation", "attributes"];

    public AnnotationCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        if (string.IsNullOrEmpty(this.Parameters.Assembly))
        {
            Logger.Error("Run from attributes can not be run without assembly parameter");
            return this.Error();
        }
        LocateAssemblyResult result = GeneratorAssemblyLocator.Locate(this.Parameters.Assembly, this.Parameters.IsBeforeBuild);
        if (result.SwitchContext)
        {
            return result;
        }
        if (this.Parameters.IsBeforeBuild && !result.Success)
        {
            return this.Success();
        }
        bool isAssemblyAsync = result.Assembly.IsAsync();
        if (!this.Parameters.IsOnlyAsync && isAssemblyAsync)
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
        foreach (Type objectType in TypeHelper.GetTypes(result.Assembly))
        {
            IDependencyResolver commandResolver = this.resolver.CloneForCommand();
            GeneratorCommandRunner generatorCommandRunner = commandResolver.Get<GeneratorCommandRunner>();
            List<Attribute> attributes = objectType.GetCustomAttributes().ToList();
            List<CliCommand> rawCommands = [];
            foreach (IGeneratorCommandAttribute attribute in attributes.OfType<IGeneratorCommandAttribute>())
            {
                rawCommands.AddRange(attribute.Commands.Select(x =>
                {
                    CliCommand command = new(x.Command);
                    command.Parameters.AddRange(x.Parameters.Select(CliCommandParameter.Parse));
                    command.Parameters.Add(new CliCommandParameter(nameof(GeneratorCommandParameters.IsAsyncAssembly), isAssemblyAsync.ToString()));
                    command.Parameters.AddRange(globalParameters);
                    foreach (CliCommandParameter parameter in command.Parameters)
                    {
                        parameter.Value = parameter.Value.Replace("$ASSEMBLY$", this.Parameters.Assembly)
                                                   .Replace("$NAMESPACE$", objectType.Namespace)
                                                   .Replace("$NAME$", objectType.FullName.TrimStart(objectType.Namespace + "."));
                    }
                    return command;
                }));
            }
            if (rawCommands.Count == 0)
            {
                continue;
            }
            foreach (IGeneratorCommandAdditionalParameterAttribute additionalParameterAttribute in attributes.OfType<IGeneratorCommandAdditionalParameterAttribute>())
            {
                foreach (AttributeCommandConfiguration additionalParameters in additionalParameterAttribute.Commands)
                {
                    foreach (CliCommand command in rawCommands.Where(x => x.Name == additionalParameters.Command || additionalParameters.Command == "*"))
                    {
                        command.Parameters.AddRange(additionalParameters.Parameters.Select(CliCommandParameter.Parse));
                    }
                }
            }
            List<IGeneratorCommand> commands = commandResolver.Get<GeneratorCommandFactory>().Create(rawCommands, commandResolver);
            commands.ForEach(x => x.Parse());
            IGeneratorCommandResult commandResult = generatorCommandRunner.Run(commands);
            if (!commandResult.Success)
            {
                return commandResult;
            }
            this.resolver.Get<IEnvironment>().TransferObjects.AddIfNotExists(commandResolver.Get<List<ITransferObject>>());
        }
        return this.Success();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Models;
using KY.Generator.Output;

namespace KY.Generator.Reflection.Commands
{
    public class RunByAttributeCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        private readonly GeneratorEnvironment environment;

        public string[] Names { get; } = { "run-by-attributes" };

        public RunByAttributeCommand(IDependencyResolver resolver, GeneratorEnvironment environment)
        {
            this.resolver = resolver;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            CommandRunner commandRunner = this.resolver.Get<CommandRunner>();
            string assemblyName = configuration.Parameters.GetString("assembly");
            if (string.IsNullOrEmpty(assemblyName))
            {
                Logger.Error("Run from attributes can not be run without assembly parameter");
                return false;
            }
            Assembly assembly = GeneratorAssemblyLocator.Locate(assemblyName, this.environment);
            if (assembly == null)
            {
                return false;
            }
            bool isAssemblyAsync = assembly.IsAsync();
            if (!this.environment.IsOnlyAsync && isAssemblyAsync)
            {
                this.environment.SwitchToAsync = true;
                return true;
            }
            foreach (Type objectType in TypeHelper.GetTypes(assembly))
            {
                List<Attribute> attributes = objectType.GetCustomAttributes().ToList();
                List<CommandConfiguration> commands = new List<CommandConfiguration>();
                foreach (IGeneratorCommandAttribute attribute in attributes.OfType<IGeneratorCommandAttribute>())
                {
                    commands.AddRange(attribute.Commands.Select(x =>
                    {
                        CommandConfiguration commandConfiguration = new CommandConfiguration(x.Command).AddParameters(x.Parameters);
                        commandConfiguration.IsAsyncAssembly = isAssemblyAsync;
                        foreach (CommandValueParameter commandParameter in commandConfiguration.Parameters.OfType<CommandValueParameter>())
                        {
                            commandParameter.Value = commandParameter.Value.Replace("$ASSEMBLY$", assemblyName)
                                                                     .Replace("$NAMESPACE$", objectType.Namespace)
                                                                     .Replace("$NAME$", objectType.FullName.TrimStart(objectType.Namespace + "."));
                        }
                        return commandConfiguration;
                    }));
                    foreach (IGeneratorCommandAdditionalParameterAttribute additionalParameterAttribute in attributes.OfType<IGeneratorCommandAdditionalParameterAttribute>())
                    {
                        foreach (AttributeCommandConfiguration additionalParameters in additionalParameterAttribute.Commands)
                        {
                            foreach (CommandConfiguration commandConfiguration in commands.Where(x => x.Command == additionalParameters.Command || additionalParameters.Command == "*"))
                            {
                                commandConfiguration.AddParameters(additionalParameters.Parameters);
                            }
                        }
                    }
                    if (commands.Count > 0)
                    {
                        commandRunner.Run(commands, output);
                        this.environment.TransferObjects.Clear();
                    }
                }
            }
            return true;
        }
    }
}
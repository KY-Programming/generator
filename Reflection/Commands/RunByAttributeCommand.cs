using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Reflection.Helpers;

namespace KY.Generator.Reflection.Commands
{
    public class RunByAttributeCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        public string[] Names { get; } = { "run-by-attributes" };

        public RunByAttributeCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
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
            Assembly assembly = GeneratorAssemblyLocator.Locate(assemblyName);
            if (assembly == null)
            {
                return false;
            }

            foreach (Type objectType in TypeHelper.GetTypes(assembly))
            {
                foreach (IGeneratorCommandAttribute attribute in objectType.GetCustomAttributes().OfType<IGeneratorCommandAttribute>())
                {
                    List<CommandConfiguration> commands = attribute.Commands.Select(x =>
                    {
                        CommandConfiguration commandConfiguration = new CommandConfiguration(x.Command).AddParameters(x.Parameters);
                        foreach (CommandValueParameter commandParameter in commandConfiguration.Parameters.OfType<CommandValueParameter>())
                        {
                            commandParameter.Value = commandParameter.Value.Replace("$NAMESPACE$", objectType.Namespace).Replace("$NAME$", objectType.Name);
                        }
                        return commandConfiguration;
                    }).ToList();
                    commandRunner.Run(commands, output);
                }
            }
            return true;
        }
    }
}
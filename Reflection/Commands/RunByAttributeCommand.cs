using System;
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
                    CommandConfiguration command = new CommandConfiguration(attribute.Command);
                    command.Parameters.Add(new CommandValueParameter("namespace", objectType.Namespace));
                    command.Parameters.Add(new CommandValueParameter("name", objectType.Name));
                    if (attribute.Parameters != null)
                    {
                        command.AddParameters(attribute.Parameters);
                    }
                    commandRunner.Run(command, output);
                }
            }
            return true;
        }
    }
}
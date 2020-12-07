using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Models;
using KY.Generator.Output;

namespace KY.Generator.Command
{
    public class CommandRunner
    {
        private readonly List<IGeneratorCommand> commands;
        private readonly GeneratorEnvironment environment;

        public CommandRunner(List<IGeneratorCommand> commands, GeneratorEnvironment environment)
        {
            this.commands = commands;
            this.environment = environment;
        }

        public bool Run(CommandConfiguration configuration, IOutput output)
        {
            if (configuration == null)
            {
                return false;
            }
            List<IGeneratorCommand> commandsToRun = this.commands.Where(x => x.Names.Any(name => name.Equals(configuration.Command, StringComparison.InvariantCultureIgnoreCase))).ToList();
            if (commandsToRun.Count == 0)
            {
                CommandNotFoundError(configuration);
                CommandDocumentationHint();
            }
            return this.Run(commandsToRun.Select(x => Tuple.Create(x, configuration)), output);
        }

        public bool Run(IEnumerable<CommandConfiguration> configurations, IOutput output)
        {
            bool allCommandsFound = true;
            List<Tuple<IGeneratorCommand, CommandConfiguration>> commandsToRun = new List<Tuple<IGeneratorCommand, CommandConfiguration>>();
            foreach (CommandConfiguration configuration in configurations)
            {
                IGeneratorCommand commandToRun = this.commands.SingleOrDefault(x => x.Names.Any(name => name.Equals(configuration.Command, StringComparison.InvariantCultureIgnoreCase)));
                if (commandToRun == null)
                {
                    allCommandsFound = false;
                    CommandNotFoundError(configuration);
                }
                else
                {
                    commandsToRun.Add(Tuple.Create(commandToRun, configuration));
                }
            }
            if (!allCommandsFound)
            {
                CommandDocumentationHint();
                return false;
            }
            return this.Run(commandsToRun, output);
        }

        private bool Run(IEnumerable<Tuple<IGeneratorCommand, CommandConfiguration>> tuples, IOutput output)
        {
            foreach ((IGeneratorCommand command, CommandConfiguration configuration) in tuples)
            {
                if (!configuration.SkipAsyncCheck)
                {
                    bool isCommandAsync = configuration.Parameters.GetBool("async");
                    if (!this.environment.IsOnlyAsync && isCommandAsync)
                    {
                        this.environment.SwitchToAsync = true;
                        continue;
                    }
                    bool? isAssemblyAsync = configuration.IsAsyncAssembly;
                    if (!isCommandAsync)
                    {
                        string assemblyName = configuration.Parameters.GetString("assembly");
                        if (!string.IsNullOrEmpty(assemblyName))
                        {
                            isAssemblyAsync = GeneratorAssemblyLocator.Locate(assemblyName, this.environment)?.IsAsync();
                        }
                    }
                    if (isAssemblyAsync != null)
                    {
                        if (!this.environment.IsOnlyAsync && isAssemblyAsync.Value)
                        {
                            this.environment.SwitchToAsync = true;
                            continue;
                        }
                        if (this.environment.IsOnlyAsync && !isCommandAsync && !isAssemblyAsync.Value)
                        {
                            continue;
                        }
                    }
                }
                bool success = command.Generate(configuration, ref output);
                if (!success)
                {
                    return false;
                }
            }
            output.Execute();
            return true;
        }

        private static void CommandDocumentationHint()
        {
            Logger.Error("See our Documentation: https://generator.ky-programming.de/");
        }

        private static void CommandNotFoundError(CommandConfiguration configuration)
        {
            Logger.Error($"Command '{configuration.Command}' not found");
        }
    }
}
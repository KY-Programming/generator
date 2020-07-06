using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Models;
using KY.Generator.Output;

namespace KY.Generator.Command
{
    public class CommandRunner
    {
        private readonly List<IGeneratorCommand> commands;

        public CommandRunner(List<IGeneratorCommand> commands)
        {
            this.commands = commands;
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
            bool success = commandsToRun.Select(x => x.Generate(configuration, ref output)).ToList().Any(x => x);
            if (success)
            {
                output.Execute();
            }
            return success;
        }

        public bool Run(IEnumerable<CommandConfiguration> configurations, IOutput output)
        {
            GeneratorEnvironment environment = new GeneratorEnvironment();
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

            foreach (Tuple<IGeneratorCommand, CommandConfiguration> tuple in commandsToRun)
            {
                if (tuple.Item1 is IUseGeneratorCommandEnvironment useGeneratorCommandEnvironment)
                {
                    useGeneratorCommandEnvironment.Environment = environment;
                }
                bool success = tuple.Item1.Generate(tuple.Item2, ref output);
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
            Logger.Error("See our Wiki on Github: https://github.com/KY-Programming/generator/wiki/v3:-Overview#commands");
        }

        private static void CommandNotFoundError(CommandConfiguration configuration)
        {
            Logger.Error($"Command '{configuration.Command}' not found");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
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
                Logger.Error($"Command '{configuration.Command}' not found");
            }
            bool success = commandsToRun.Select(x => x.Generate(configuration, ref output)).ToList().Any(x => x);
            if (success)
            {
                output.Execute();
            }
            return success;
        }
    }
}
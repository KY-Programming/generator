using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Command
{
    public class CommandRunner
    {
        private readonly List<IGeneratorCommand> allCommands;

        public CommandRunner(List<IGeneratorCommand> allCommands)
        {
            this.allCommands = allCommands;
        }

        public List<IGeneratorCommand> Convert(IEnumerable<RawCommand> commands, List<ITransferObject> transferObjects = null)
        {
            bool allCommandsFound = true;
            List<IGeneratorCommand> foundCommands = new List<IGeneratorCommand>();
            foreach (RawCommand rawCommand in commands)
            {
                IGeneratorCommand command = this.FindCommand(rawCommand.Name);
                if (command == null)
                {
                    allCommandsFound = false;
                    GeneratorErrors.CommandNotFoundError(rawCommand);
                }
                else
                {
                    command.Parse(rawCommand.Parameters);
                    command.TransferObjects = transferObjects;
                    foundCommands.Add(command);
                }
            }
            if (!allCommandsFound)
            {
                GeneratorErrors.CommandDocumentationHint();
            }
            return foundCommands;
        }
        

        public IGeneratorCommand FindCommand(string command)
        {
            return this.allCommands.SingleOrDefault(x => x.Names.Any(name => name.Equals(command, StringComparison.InvariantCultureIgnoreCase)));
        }

        public IGeneratorCommandResult Run(IEnumerable<RawCommand> commands, IOutput output, List<ITransferObject> transferObjects)
        {
            return this.Run(this.Convert(commands, transferObjects), output);
        }

        public IGeneratorCommandResult Run(RawCommand command, IOutput output, List<ITransferObject> transferObjects)
        {
            return this.Run(command.Yield(), output, transferObjects);
        }

        public IGeneratorCommandResult Run(IEnumerable<IGeneratorCommand> commands, IOutput output)
        {
            IGeneratorCommandResult result = null;
            foreach (IGeneratorCommand command in commands)
            {
                result = command.Run(output);
                if (!result.Success)
                {
                    return result;
                }
            }
            return result ?? new SuccessResult();
        }

        public IGeneratorCommandResult Run(IGeneratorCommand command, IOutput output)
        {
            if (!string.IsNullOrEmpty(command.Parameters.Output))
            {
                output.Move(command.Parameters.Output);
            }
            if (!command.Parameters.SkipAsyncCheck)
            {
                if (!command.Parameters.IsOnlyAsync && command.Parameters.IsAsync)
                {
                    return new SwitchAsyncResult();
                }
                bool? isAssemblyAsync = command.Parameters.IsAsyncAssembly;
                if (!command.Parameters.IsAsync)
                {
                    if (!string.IsNullOrEmpty(command.Parameters.Assembly))
                    {
                        LocateAssemblyResult locateAssemblyResult = GeneratorAssemblyLocator.Locate(command.Parameters.Assembly, command.Parameters.IsBeforeBuild);
                        if (locateAssemblyResult.SwitchContext)
                        {
                            return locateAssemblyResult;
                        }
                        isAssemblyAsync = locateAssemblyResult.Assembly?.IsAsync();
                    }
                }
                if (isAssemblyAsync != null)
                {
                    if (!command.Parameters.IsOnlyAsync && isAssemblyAsync.Value)
                    {
                        return new SwitchAsyncResult();
                    }
                    if (command.Parameters.IsOnlyAsync && !command.Parameters.IsAsync && !isAssemblyAsync.Value)
                    {
                        return new SwitchAsyncResult();
                    }
                }
            }
            return command.Run(output);
        }
    }
}
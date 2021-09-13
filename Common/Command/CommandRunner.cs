using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Extensions;
using KY.Generator.Output;
using KY.Generator.Statistics;

namespace KY.Generator.Command
{
    public class CommandRunner
    {
        private readonly IDependencyResolver resolver;
        private readonly IOutput output;
        private readonly StatisticsService statisticsService;

        public CommandRunner(IDependencyResolver resolver, IOutput output, StatisticsService statisticsService)
        {
            this.resolver = resolver;
            this.output = output;
            this.statisticsService = statisticsService;
        }

        public List<IGeneratorCommand> Convert(IEnumerable<RawCommand> commands)
        {
            bool allCommandsFound = true;
            List<IGeneratorCommand> foundCommands = new();
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
            return this.resolver.Get<List<IGeneratorCommand>>().SingleOrDefault(x => x.Names.Any(name => name.Equals(command, StringComparison.InvariantCultureIgnoreCase)));
        }

        public IGeneratorCommandResult Run(IEnumerable<RawCommand> commands)
        {
            return this.Run(this.Convert(commands));
        }

        public IGeneratorCommandResult Run(RawCommand command)
        {
            return this.Run(command.Yield());
        }

        public IGeneratorCommandResult Run(IEnumerable<IGeneratorCommand> commands)
        {
            List<IGeneratorCommand> list = commands.ToList();
            IGeneratorCommandResult result = null;
            list.ForEach(command => command.Prepare());
            foreach (IGeneratorCommand command in list)
            {
                result = this.Run(command);
                if (!result.Success)
                {
                    return result;
                }
            }
            return result ?? new SuccessResult();
        }

        public IGeneratorCommandResult Run(IGeneratorCommand command)
        {
            if (!string.IsNullOrEmpty(command.Parameters.Output))
            {
                this.output.Move(command.Parameters.Output);
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
            Measurement measurement = this.statisticsService.StartMeasurement();
            try
            {
                return command.Run();
            }
            finally
            {
                this.statisticsService.Measure(measurement, command);
            }
        }
    }
}

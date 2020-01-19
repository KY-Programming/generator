using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator
{
    public class ConfigurationRunner
    {
        private readonly CommandRegistry commands;

        public ConfigurationRunner(CommandRegistry commands)
        {
            this.commands = commands;
        }

        public bool Run(IEnumerable<IConfiguration> allConfigurations, bool isBeforeBuild = false)
        {
            List<IConfiguration> configurations = allConfigurations.Where(x => x.BeforeBuild == isBeforeBuild).ToList();
            Logger.Trace($"Start generating {configurations.Count} configurations");
            IConfiguration missingLanguage = configurations.OfType<IConfigurationWithLanguage>().FirstOrDefault(x => x.Language == null);
            if (missingLanguage != null)
            {
                Logger.Error($"Configuration '{missingLanguage.GetType().Name}' without language found. Generation failed!");
                return false;
            }
            try
            {
                List<ITransferObject> transferObjects = new List<ITransferObject>();
                foreach (IConfiguration configuration in configurations)
                {
                    bool success;
                    ICommand command = this.commands.CreateCommand(configuration);
                    if (command is IConfigurationCommand configurationCommand)
                    {
                        success = configurationCommand.Execute(configuration, transferObjects);
                    }
                    else if (command is ICommandLineCommand commandLineCommand)
                    {
                        success = commandLineCommand.Execute(configuration);
                    }
                    else
                    {
                        Logger.Trace($"{command.GetType().Name} can not be executed. Implement at least {nameof(IConfigurationCommand)} or {nameof(ICommandLineCommand)}");
                        success = false;
                    }
                    if (!success)
                    {
                        return false;
                    }
                }
                configurations.Where(x => x.Output != null).Select(x => x.Output).Unique().ForEach(x => x.Execute());
                return true;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return false;
            }
        }
    }
}
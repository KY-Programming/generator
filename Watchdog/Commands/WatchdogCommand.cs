using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Watchdog.Helpers;
using KY.Generator.Watchdog.Watchdogs;

namespace KY.Generator.Watchdog.Commands
{
    public class WatchdogCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        public string[] Names { get; } = { "watchdog" };

        public WatchdogCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Generate(CommandConfiguration configuration, IOutput output)
        {
            Logger.Trace("Execute watchdog command...");
            if (configuration.Parameters.GetBool("async"))
            {
                Logger.Trace("Start generation in separate process...");
                if (InstanceHelper.IsRunning())
                {
                    Logger.Trace("Generation aborted. An other watchdog is already running.");
                    return true;
                }
                string arguments = string.Join(" ", configuration.Parameters.Where(x => x.Name != "async"));
                ProcessStartInfo startInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().Location, $"watchdog {arguments}");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = FileSystem.Parent(startInfo.FileName);
                Logger.Trace($"{startInfo.FileName} {startInfo.Arguments}");
                Process.Start(startInfo);
                return true;
            }

            string url = configuration.Parameters.GetString("url");
            string launchSettings = configuration.Parameters.GetString("launchSettings");
            TimeSpan timeout = configuration.Parameters.GetTimeSpan("timeout", TimeSpan.FromMinutes(5));
            TimeSpan delay = configuration.Parameters.GetTimeSpan("delay", TimeSpan.FromSeconds(1));
            TimeSpan sleep = configuration.Parameters.GetTimeSpan("sleep", TimeSpan.FromMilliseconds(100));
            int tries = configuration.Parameters.GetInt("tries");

            string command = configuration.Parameters.GetString("command");
            if (string.IsNullOrEmpty(command))
            {
                Logger.Error("Command can not be empty");
                return false;
            }
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(launchSettings))
            {
                Logger.Error("No valid target found. Add at least a -url=... or a -launchSettings=... parameter");
                return false;
            }
            if (!string.IsNullOrEmpty(launchSettings))
            {
                LaunchSettingsReader reader = new LaunchSettingsReader();
                url = reader.ReadApplicationUrl(launchSettings);
                if (string.IsNullOrEmpty(url))
                {
                    Logger.Error("No value for iisSettings/iisExpress/applicationUrl in launchSettings.json found");
                    return false;
                }
                url += "/api/v1/generator/available";
            }
            HttpWatchdog watchdog = new HttpWatchdog(url, tries, delay, sleep, timeout);
            bool success = watchdog.WaitAsync().Result;
            if (success)
            {
                CommandConfiguration nextCommand = new CommandConfiguration(command);
                nextCommand.CopyBaseFrom(configuration);
                nextCommand.Parameters.AddRange(configuration.Parameters.Where(x => x.Name.StartsWith(command)).Select(x => this.MapParameter(x, command)));
                nextCommand.ReadFromParameters(nextCommand.Parameters, this.resolver.Get<List<ILanguage>>());

                this.resolver.Get<CommandRunner>().Run(nextCommand, output);
            }
            return true;
        }

        private CommandParameter MapParameter(CommandParameter parameter, string command)
        {
            string name = parameter.Name.TrimStart($"{command}-");
            if (parameter is CommandValueParameter valueParameter)
            {
                return new CommandValueParameter(name, valueParameter.Value);
            }
            return new CommandParameter(name);
        }
    }
}
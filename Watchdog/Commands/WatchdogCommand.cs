using System;
using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Watchdog.Helpers;
using KY.Generator.Watchdog.Watchdogs;

namespace KY.Generator.Watchdog.Commands
{
    public class WatchdogCommandParameters : GeneratorCommandParameters
    {
        public string Url { get; set; }
        public string LaunchSettings { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan Sleep { get; set; } = TimeSpan.FromMilliseconds(100);
        public int Tries { get; set; }
        public string Command { get; set; }
    }

    public class WatchdogCommand : GeneratorCommand<WatchdogCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "watchdog" };

        public WatchdogCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            Logger.Trace("Execute watchdog command...");
            if (this.Parameters.IsAsync)
            {
                Logger.Trace("Start generation in separate process...");
                if (InstanceHelper.IsRunning())
                {
                    Logger.Trace("Generation aborted. An other watchdog is already running.");
                    return this.Success();
                }
                string arguments = string.Join(" ", this.Parameters);
                ProcessStartInfo startInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().Location, $"watchdog {arguments}");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = FileSystem.Parent(startInfo.FileName);
                Logger.Trace($"{startInfo.FileName} {startInfo.Arguments}");
                Process.Start(startInfo);
                return this.Success();
            }

            string url = this.Parameters.Url;
            string launchSettings = this.Parameters.LaunchSettings;
            TimeSpan timeout = this.Parameters.Timeout;
            TimeSpan delay = this.Parameters.Delay;
            TimeSpan sleep = this.Parameters.Sleep;
            int tries = this.Parameters.Tries;

            string command = this.Parameters.Command;
            if (string.IsNullOrEmpty(command))
            {
                Logger.Error("Command can not be empty");
                return this.Error();
            }
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(launchSettings))
            {
                Logger.Error("No valid target found. Add at least a -url=... or a -launchSettings=... parameter");
                return this.Error();
            }
            if (!string.IsNullOrEmpty(launchSettings))
            {
                LaunchSettingsReader reader = new LaunchSettingsReader();
                url = reader.ReadApplicationUrl(launchSettings);
                if (string.IsNullOrEmpty(url))
                {
                    Logger.Error("No value for iisSettings/iisExpress/applicationUrl in launchSettings.json found");
                    return this.Error();
                }
                url += "/api/v1/generator/available";
            }
            HttpWatchdog watchdog = new HttpWatchdog(url, tries, delay, sleep, timeout);
            bool success = watchdog.WaitAsync().Result;
            if (success)
            {
                throw new NotImplementedException();
                //CommandConfiguration nextCommand = new CommandConfiguration(command);
                //nextCommand.Parameters.AddRange(configuration.Parameters.Where(x => x.Name.StartsWith(command)).Select(x => this.MapParameter(x, command)));
                //nextCommand.ReadFromParameters(nextCommand.Parameters, this.resolver.Get<List<ILanguage>>());

                //this.resolver.Get<CommandRunner>().Run(nextCommand, output);
            }
            return this.Success();
        }

        //private ICommandParameter MapParameter(ICommandParameter parameter, string command)
        //{
        //    string name = parameter.Name.TrimStart($"{command}-");
        //    if (parameter is CommandValueParameter valueParameter)
        //    {
        //        return new CommandValueParameter(name, valueParameter.Value);
        //    }
        //    return new CommandParameter(name);
        //}
    }
}
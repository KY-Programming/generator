using KY.Core;
using KY.Generator.Command;
using KY.Generator.Watchdog.Watchdogs;

namespace KY.Generator.Watchdog.Commands;

internal class WatchdogCommand : GeneratorCommand<WatchdogCommandParameters>
{
    public override async Task<IGeneratorCommandResult> Run()
    {
        Logger.Trace("Execute watchdog command...");

        string? url = this.Parameters.Url;
        string? launchSettings = this.Parameters.LaunchSettings;
        TimeSpan timeout = this.Parameters.Timeout;
        TimeSpan delay = this.Parameters.Delay;
        TimeSpan sleep = this.Parameters.Sleep;
        int tries = this.Parameters.Tries;

        if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(launchSettings))
        {
            Logger.Error("No valid target found. Add at least a -url=... or a -launchSettings=... parameter");
            return this.Error();
        }
        if (!string.IsNullOrEmpty(launchSettings))
        {
            LaunchSettingsReader reader = new();
            url = reader.ReadApplicationUrl(launchSettings);
            if (string.IsNullOrEmpty(url))
            {
                Logger.Error("No value for iisSettings/iisExpress/applicationUrl in launchSettings.json found");
                return this.Error();
            }
            url += "/api/v1/generator/available";
        }
        HttpWatchdog watchdog = new(url, tries, delay, sleep, timeout);
        bool success = await watchdog.WaitAsync();
        if (!success)
        {
            // Everything declared behind the watchdog waits for this target, so there is nothing left to
            // run - the failure stops the chain instead of letting the following commands fail one by one
            // against a service that is not there.
            Logger.Error($"Wait for {url} failed. Nothing is generated");
            return this.Error();
        }
        return this.Success();
    }
}

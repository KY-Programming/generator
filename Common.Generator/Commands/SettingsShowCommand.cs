using KY.Core;
using KY.Generator.Command;

namespace KY.Generator.Commands;

/// <summary>
/// Prints the settings files that apply and the value every option ends up with, together with the file that
/// set it. The answer to "why does my output suddenly look different"
/// </summary>
internal class SettingsShowCommand(SettingsService settingsService, GeneratorModuleLoader moduleLoader) : GeneratorCommand<SettingsShowCommandParameters>
{
    public override Task<IGeneratorCommandResult> Run()
    {
        // Without them only the sections of the modules this run happens to have loaded would be listed
        moduleLoader.LoadShipped();
        settingsService.Apply();
        // Every module is loaded, so an unknown section really is one
        settingsService.WarnAboutUnknownKeys(true);
        Logger.Trace($"Settings for {settingsService.StartPath}");
        if (settingsService.Files.Count == 0)
        {
            Logger.Trace("  no settings file found");
        }
        else
        {
            Logger.Trace("  Files (the last one wins):");
            foreach (SettingsFile file in settingsService.Files)
            {
                List<string> marker = [];
                if (file.IsGlobal)
                {
                    marker.Add("global");
                }
                if (file.IsRoot)
                {
                    marker.Add("root");
                }
                if (file.IgnoresGlobalSettings)
                {
                    marker.Add("ignores the settings of this machine");
                }
                Logger.Trace($"    {file.Path}{(marker.Count == 0 ? string.Empty : $" ({string.Join(", ", marker)})")}");
            }
        }
        Logger.Trace("  Values:");
        this.Write("api", settingsService.Api);
        this.Write("statistics", settingsService.Statistics ? "true" : "false");
        // No file naming it means it is the id of this installation, not a default anybody configured
        this.Write("license", settingsService.License.ToString(), settingsService.GetSource("license") ?? "id of this installation");
        this.Write("certificate", settingsService.Certificate == null ? null : "<set>");
        foreach (IConfigurableOptionsFactory factory in settingsService.GetFactories().OrderBy(x => x.SettingsSection))
        {
            this.Write(factory.SettingsSection, Options.GetGlobal(factory.OptionsType), factory.SettingsOptions);
        }
        return this.SuccessAsync();
    }

    private void Write(string path, object options, IReadOnlyList<SettingsOption> settingsOptions)
    {
        foreach (SettingsOption option in settingsOptions)
        {
            if (option.Children.Count > 0)
            {
                object? childTarget = option.SelectChildTarget(options);
                if (childTarget != null)
                {
                    this.Write($"{path}.{option.Name}", childTarget, option.Children);
                }
                continue;
            }
            this.Write($"{path}.{option.Name}", Format(option.Read(options)));
        }
    }

    private void Write(string key, string? value, string? source = null)
    {
        source ??= settingsService.GetSource(key) ?? "default";
        Logger.Trace($"    {key.PadRight(42)} = {(value ?? "<not set>").PadRight(28)} ({source})");
    }

    private static string? Format(object? value)
    {
        return value switch
        {
            null => null,
            bool boolean => boolean ? "true" : "false",
            IReadOnlyDictionary<string, string> dictionary => dictionary.Count == 0
                                                                  ? null
                                                                  : string.Join(", ", dictionary.Select(entry => $"{(string.IsNullOrEmpty(entry.Key) ? "*" : entry.Key)}: {entry.Value}")),
            _ => value.ToString()
        };
    }
}

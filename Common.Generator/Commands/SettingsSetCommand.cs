using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Commands;

/// <summary>
/// Writes one option into a <c>ky-generator.json</c>
/// </summary>
internal class SettingsSetCommand(SettingsService settingsService, GeneratorModuleLoader moduleLoader) : GeneratorCommand<SettingsSetCommandParameters>
{
    public override Task<IGeneratorCommandResult> Run()
    {
        // Without them an option of a module would look unknown, only because this run has no project that uses it
        moduleLoader.LoadShipped();
        if (string.IsNullOrWhiteSpace(this.Parameters.Key))
        {
            Logger.Error($"No option to set. Use -key=<option> -value=<value>, e.g. -key=options.addHeader -value=false. Run '{SettingsShowCommandParameters.Names.First()}' to see every option");
            return this.ErrorAsync();
        }
        string key = this.Parameters.Key!;
        SettingsOption? option = settingsService.FindOption(key, out bool isOwnKey);
        if (option == null && !isOwnKey)
        {
            Logger.Error($"Unknown option '{key}'. Known options: {string.Join(", ", settingsService.GetKnownKeys())}");
            return this.ErrorAsync();
        }
        JToken? value = Parse(this.Parameters.Value, option);
        if (value == null)
        {
            Logger.Error($"'{this.Parameters.Value}' is not a valid value for '{key}'{(option == null ? string.Empty : $". Expected {option.ValueType.Name}")}");
            return this.ErrorAsync();
        }
        string path = settingsService.GetWritablePath(this.Parameters.Global, this.Parameters.Path);
        this.WarnIfNextToAProject(path);
        if (!SettingsWriter.TryLoad(path, out JObject content, out string? error))
        {
            Logger.Error($"{path} is not valid json, so writing it would replace everything in it with this one value. Fix it first - '{SettingsValidateCommandParameters.Names.First()}' points at the problem. {error}");
            return this.ErrorAsync();
        }
        if (SettingsWriter.HasComments(path))
        {
            Logger.Warning($"The comments in {path} are lost by writing it. Edit the file by hand to keep them");
        }
        SettingsWriter.Set(content, key, value);
        SettingsWriter.Save(path, content);
        Logger.Trace($"{key} = {value} written to {path}");
        return this.SuccessAsync();
    }

    /// <summary>
    /// The search for settings files starts above the project, so a file next to one is never read for it
    /// </summary>
    private void WarnIfNextToAProject(string path)
    {
        string directory = FileSystem.Parent(path);
        if (!FileSystem.DirectoryExists(directory))
        {
            return;
        }
        string[] projects = FileSystem.GetFiles(directory, "*.csproj");
        if (projects.Length > 0)
        {
            Logger.Warning($"{path} sits next to {FileSystem.GetFileName(projects.First())}. A settings file is only read from the directories above a project - use the assembly attributes to configure the project itself");
        }
    }

    private static JToken? Parse(string? value, SettingsOption? option)
    {
        if (value == null)
        {
            return null;
        }
        JToken token;
        try
        {
            // So -value=false becomes a boolean and -value=2 a number, while -value=Output/Models stays a string
            token = JToken.Parse(value);
        }
        catch (Exception)
        {
            token = new JValue(value);
        }
        if (option == null)
        {
            return token;
        }
        try
        {
            token.ToObject(option.ValueType);
            return token;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

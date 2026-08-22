using KY.Core;
using KY.Generator.Command;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Commands;

/// <summary>
/// Checks every <c>ky-generator.json</c> that applies: that it is valid json, that every key is one the generator
/// knows and that every value fits the option it is written to. A settings file is written by hand, so a typo in one
/// would otherwise only show up as output that silently looks different.
/// <para>
/// It also reports the entries that change nothing, because the value they set is the one that would apply anyway
/// </para>
/// </summary>
internal class SettingsValidateCommand(SettingsService settingsService, GeneratorModuleLoader moduleLoader) : GeneratorCommand<SettingsValidateCommandParameters>
{
    private readonly List<string> errors = [];
    private readonly List<string> warnings = [];
    private readonly List<string> notes = [];

    public override Task<IGeneratorCommandResult> Run()
    {
        // Without them every section of a module that this run does not use would look like a typo
        moduleLoader.LoadShipped();
        if (settingsService.Files.Count == 0)
        {
            Logger.Trace($"No settings file applies to {settingsService.StartPath}. Nothing to check");
            return this.SuccessAsync();
        }
        // What would apply if a file did not set the key, so an entry that repeats it can be reported as pointless.
        // Starts at the built-in defaults and grows while the files are walked from the outermost to the innermost
        Dictionary<string, JToken?> effective = new(StringComparer.OrdinalIgnoreCase);
        foreach (SettingsFile file in settingsService.Files)
        {
            Logger.Trace($"Checking {file.Path}...");
            if (file.Error != null)
            {
                this.errors.Add($"{file.Path} is not valid json. {file.Error}");
                continue;
            }
            this.Check(file, file.Content, effective);
        }
        this.Report();
        return this.errors.Count == 0 ? this.SuccessAsync() : this.ErrorAsync();
    }

    private void Check(SettingsFile file, JObject content, Dictionary<string, JToken?> effective)
    {
        foreach (JProperty property in content.Properties())
        {
            if (property.Name.Equals(GeneratorSettings.SchemaKey, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            SettingsOption? own = GeneratorSettings.Options.FirstOrDefault(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
            if (own != null)
            {
                this.CheckValue(file, own, property.Name, property.Value, effective);
                continue;
            }
            IConfigurableOptionsFactory? factory = settingsService.GetFactories()
                                                                  .FirstOrDefault(x => x.SettingsSection.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
            if (factory == null)
            {
                this.warnings.Add($"{file.Path}: unknown entry '{property.Name}'. It is ignored - a file written for a newer generator keeps working");
                continue;
            }
            if (property.Value is JObject section)
            {
                this.Check(file, factory.SettingsSection, section, factory.SettingsOptions, effective);
            }
            else
            {
                this.errors.Add($"{file.Path}: '{property.Name}' has to be an object");
            }
        }
    }

    private void Check(SettingsFile file, string path, JObject section, IReadOnlyList<SettingsOption> options, Dictionary<string, JToken?> effective)
    {
        foreach (JProperty property in section.Properties())
        {
            SettingsOption? option = options.FirstOrDefault(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
            if (option == null)
            {
                this.warnings.Add($"{file.Path}: unknown option '{path}.{property.Name}'");
                continue;
            }
            if (option.Children.Count > 0)
            {
                if (property.Value is JObject children)
                {
                    this.Check(file, $"{path}.{option.Name}", children, option.Children, effective);
                }
                else
                {
                    this.errors.Add($"{file.Path}: '{path}.{option.Name}' has to be an object");
                }
                continue;
            }
            this.CheckValue(file, option, $"{path}.{option.Name}", property.Value, effective);
        }
    }

    private void CheckValue(SettingsFile file, SettingsOption option, string key, JToken value, Dictionary<string, JToken?> effective)
    {
        try
        {
            value.ToObject(option.ValueType);
        }
        catch (Exception exception)
        {
            this.errors.Add($"{file.Path}: '{key}' is not a valid {Describe(option.ValueType)}. {exception.Message}");
            return;
        }
        if (!effective.TryGetValue(key, out JToken? previous))
        {
            previous = option.DefaultValue == null ? null : JToken.FromObject(option.DefaultValue);
        }
        // A dictionary is merged key by key, so comparing the whole object would call an entry pointless that only
        // repeats one of its keys. Not worth getting wrong, the ones that matter are the plain values
        if (option.ValueType != typeof(JObject) && !IsDictionary(option.ValueType) && JToken.DeepEquals(value, previous))
        {
            this.notes.Add($"{file.Path}: '{key}' is already {value.ToString(Newtonsoft.Json.Formatting.None)} without it. The entry can be removed");
        }
        effective[key] = value;
    }

    private void Report()
    {
        this.notes.ForEach(note => Logger.Trace("  " + note));
        this.warnings.ForEach(warning => Logger.Warning(warning));
        this.errors.ForEach(error => Logger.Error(error));
        string checkedFiles = $"{settingsService.Files.Count} settings file(s) checked";
        if (this.errors.Count > 0)
        {
            Logger.Error($"{checkedFiles}: {this.errors.Count} error(s), {this.warnings.Count} warning(s)");
            return;
        }
        Logger.Trace($"{checkedFiles}: no errors, {this.warnings.Count} warning(s), {this.notes.Count} entry(s) that change nothing");
    }

    private static bool IsDictionary(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
    }

    private static string Describe(Type type)
    {
        Type underlying = Nullable.GetUnderlyingType(type) ?? type;
        if (underlying.IsEnum)
        {
            return $"value, expected one of {string.Join(", ", Enum.GetNames(underlying))}";
        }
        return underlying == typeof(bool) ? "boolean"
            : underlying == typeof(int) || underlying == typeof(long) ? "number"
            : underlying == typeof(Guid) ? "guid"
            : IsDictionary(underlying) ? "object of texts"
            : "text";
    }
}

using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Commands;

/// <summary>
/// Writes the json schema of the <c>ky-generator.json</c>, so an editor can complete and validate one. It is
/// generated from the same option declarations that read the file, which is the only way to keep the two from
/// drifting apart
/// </summary>
internal class SettingsSchemaCommand(SettingsService settingsService, GeneratorModuleLoader moduleLoader) : GeneratorCommand<SettingsSchemaCommandParameters>
{
    public const string FileName = "schema.json";

    public override Task<IGeneratorCommandResult> Run()
    {
        // Without them the schema would only describe the sections of the modules this run happens to have loaded
        moduleLoader.LoadShipped();
        JObject properties = new() { [GeneratorSettings.SchemaKey] = Property("string", "The schema this file is written against") };
        foreach (SettingsOption option in GeneratorSettings.Options)
        {
            properties[option.Name] = Property(option);
        }
        foreach (IConfigurableOptionsFactory factory in settingsService.GetFactories().OrderBy(x => x.SettingsSection))
        {
            properties[factory.SettingsSection] = Object(factory.SettingsSectionDescription, factory.SettingsOptions);
        }
        JObject schema = new()
        {
            ["$schema"] = "http://json-schema.org/draft-07/schema#",
            ["title"] = "KY.Generator settings",
            ["description"] = "Options that apply to every project below the directory this file sits in",
            ["type"] = "object",
            ["additionalProperties"] = false,
            ["properties"] = properties
        };
        // A relative path has to be resolved against the current directory: the file system resolves one against the
        // base directory of the application, which would write the schema next to the executable
        string path = string.IsNullOrWhiteSpace(this.Parameters.Output)
                          ? FileSystem.Combine(settingsService.StartPath, FileName)
                          : FileSystem.IsAbsolute(this.Parameters.Output!)
                              ? this.Parameters.Output!
                              : FileSystem.Combine(Environment.CurrentDirectory, this.Parameters.Output!);
        SettingsWriter.Save(path, schema);
        Logger.Trace($"Schema written to {path}");
        return this.SuccessAsync();
    }

    private static JObject Object(string description, IReadOnlyList<SettingsOption> options)
    {
        JObject properties = new();
        foreach (SettingsOption option in options)
        {
            properties[option.Name] = option.Children.Count > 0
                                          ? Object(option.Description, option.Children)
                                          : Property(option);
        }
        return new JObject
        {
            ["type"] = "object",
            ["description"] = description,
            ["additionalProperties"] = false,
            ["properties"] = properties
        };
    }

    private static JObject Property(string type, string description)
    {
        return new JObject { ["type"] = type, ["description"] = description };
    }

    private static JObject Property(SettingsOption option)
    {
        Type type = Nullable.GetUnderlyingType(option.ValueType) ?? option.ValueType;
        if (type.IsEnum)
        {
            JObject enumeration = Property("string", option.Description);
            enumeration["enum"] = new JArray(Enum.GetNames(type).Cast<object>().ToArray());
            return enumeration;
        }
        if (type == typeof(Dictionary<string, string>))
        {
            JObject dictionary = Property("object", option.Description);
            dictionary["additionalProperties"] = new JObject { ["type"] = "string" };
            return dictionary;
        }
        return Property(ToSchemaType(type), option.Description);
    }

    private static string ToSchemaType(Type type)
    {
        if (type == typeof(bool))
        {
            return "boolean";
        }
        if (type == typeof(int) || type == typeof(long))
        {
            return "integer";
        }
        return type == typeof(double) ? "number" : "string";
    }
}

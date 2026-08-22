using System.Text;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Commands;

/// <summary>
/// Writes a <c>ky-generator.json</c> that spells out every option with the value the generator uses by default, so
/// the file itself is the documentation of what can be configured
/// </summary>
internal class SettingsInitCommand(SettingsService settingsService, GeneratorModuleLoader moduleLoader) : GeneratorCommand<SettingsInitCommandParameters>
{
    private const string Indent = "  ";

    public override Task<IGeneratorCommandResult> Run()
    {
        // Without them the file would only describe the sections of the modules this run happens to have loaded
        moduleLoader.LoadShipped();
        string path = settingsService.GetWritablePath(this.Parameters.Global, this.Parameters.Path);
        if (FileSystem.FileExists(path) && !this.Parameters.Force)
        {
            Logger.Error($"{path} already exists. Use -force to overwrite it");
            return this.ErrorAsync();
        }
        SettingsWriter.Save(path, this.Build());
        Logger.Trace($"Settings written to {path}");
        return this.SuccessAsync();
    }

    private string Build()
    {
        List<Entry> entries = [Value(GeneratorSettings.SchemaKey, SettingsService.SchemaUrl, "Lets an editor complete and check this file")];
        entries.AddRange(GeneratorSettings.Options.Where(option => !option.Hidden).Select(ToEntry));
        entries.AddRange(settingsService.GetFactories()
                                        .OrderBy(factory => factory.SettingsSection)
                                        .Select(factory => Object(factory.SettingsSection, factory.SettingsSectionDescription, factory.SettingsOptions)));
        StringBuilder builder = new();
        builder.AppendLine("{");
        builder.AppendLine($"{Indent}// Every value below is the one the generator uses anyway. Delete what you do not want to pin,");
        builder.AppendLine($"{Indent}// the options without a default are commented out and show an example instead. This is jsonc, comments are fine");
        builder.AppendLine($"{Indent}// Run '{SettingsValidateCommandParameters.Names.First()}' after editing to check the file and to find the values that are already the default");
        foreach (string line in Join(entries))
        {
            builder.AppendLine(Indent + line);
        }
        builder.AppendLine("}");
        return builder.ToString();
    }

    /// <summary>
    /// An option without a default is written as a comment, with an example instead of its value: it is there to be
    /// found and uncommented, and the example is what shows the shape it expects
    /// </summary>
    private static Entry Value(string name, object? value, string description, object? example = null)
    {
        // An enum is written as its name - the number it happens to have is nothing anybody should have to look up
        object? json = (value ?? example) is Enum enumeration ? enumeration.ToString() : value ?? example;
        string written = json == null ? "null" : JToken.FromObject(json).ToString(Formatting.None);
        return new Entry(description, [$"\"{name}\": {written}"], value != null);
    }

    private static Entry Object(string name, string description, IReadOnlyList<SettingsOption> options)
    {
        List<string> lines = [$"\"{name}\": {{"];
        lines.AddRange(Join(options.Select(ToEntry).ToList()).Select(line => Indent + line));
        lines.Add("}");
        return new Entry(description, lines, true);
    }

    private static Entry ToEntry(SettingsOption option)
    {
        return option.Children.Count > 0
                   ? Object(option.Name, option.Description, option.Children)
                   : Value(option.Name, option.DefaultValue, option.Description, option.Example);
    }

    /// <summary>
    /// Puts the entries one after the other and separates them by a comma. A commented out entry gets none - it is
    /// not there as far as the json is concerned - and neither does the last one that is
    /// </summary>
    private static List<string> Join(List<Entry> entries)
    {
        int last = entries.FindLastIndex(entry => entry.IsActive);
        List<string> result = [];
        for (int index = 0; index < entries.Count; index++)
        {
            Entry entry = entries[index];
            if (!string.IsNullOrEmpty(entry.Description))
            {
                result.Add($"// {entry.Description}");
            }
            for (int line = 0; line < entry.Lines.Count; line++)
            {
                bool isLastLine = line == entry.Lines.Count - 1;
                string comma = isLastLine && entry.IsActive && index < last ? "," : string.Empty;
                result.Add((entry.IsActive ? string.Empty : "// ") + entry.Lines[line] + comma);
            }
        }
        return result;
    }

    private class Entry(string description, List<string> lines, bool isActive)
    {
        public string Description { get; } = description;
        public List<string> Lines { get; } = lines;
        public bool IsActive { get; } = isActive;
    }
}

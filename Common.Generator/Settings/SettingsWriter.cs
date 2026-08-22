using KY.Core.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator;

/// <summary>
/// Reads and writes a <c>ky-generator.json</c> for the <c>settings-set</c> and <c>settings-init</c> commands. Nothing
/// else in the generator writes a settings file - a build only ever reads them
/// </summary>
internal static class SettingsWriter
{
    public static JObject Load(string path)
    {
        return FileSystem.FileExists(path) ? JObject.Parse(FileSystem.ReadAllText(path)) : new JObject();
    }

    /// <summary>
    /// <see cref="Load"/> for the commands that go on to write the file back. A file that does not parse must never
    /// be written: everything in it would be replaced by the one value that is being set
    /// </summary>
    public static bool TryLoad(string path, out JObject content, out string? error)
    {
        error = null;
        try
        {
            content = Load(path);
            return true;
        }
        catch (Exception exception)
        {
            content = new JObject();
            error = exception.Message;
            return false;
        }
    }

    /// <summary>
    /// A json object holds nothing but its properties, so a comment between them is lost as soon as the file is read
    /// into one and written back. Rewriting the file without touching what is around the one changed value would
    /// take a parser of its own - reporting the loss is the honest way out
    /// </summary>
    public static bool HasComments(string path)
    {
        if (!FileSystem.FileExists(path))
        {
            return false;
        }
        using JsonTextReader reader = new(new StringReader(FileSystem.ReadAllText(path)));
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.Comment)
            {
                return true;
            }
        }
        return false;
    }

    public static void Save(string path, JObject content)
    {
        Save(path, content.ToString(Formatting.Indented) + Environment.NewLine);
    }

    /// <summary>
    /// Creates the directory on the way, so a path that names one that does not exist yet - e.g. the output of
    /// <c>settings-schema</c> in a docs folder - writes instead of failing
    /// </summary>
    public static void Save(string path, string content)
    {
        string directory = FileSystem.Parent(path);
        if (!string.IsNullOrEmpty(directory) && !FileSystem.DirectoryExists(directory))
        {
            FileSystem.CreateDirectory(directory);
        }
        FileSystem.WriteAllText(path, content);
    }

    /// <summary>
    /// Writes a value under a dotted key, creating the objects on the way
    /// </summary>
    public static void Set(JObject content, string key, JToken value)
    {
        string[] segments = key.Split('.');
        JObject target = content;
        foreach (string segment in segments.Take(segments.Length - 1))
        {
            JProperty? property = target.Properties().FirstOrDefault(x => x.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));
            if (property?.Value is JObject existing)
            {
                target = existing;
                continue;
            }
            JObject created = new();
            if (property == null)
            {
                target.Add(segment, created);
            }
            else
            {
                property.Value = created;
            }
            target = created;
        }
        string last = segments[segments.Length - 1];
        JProperty? found = target.Properties().FirstOrDefault(x => x.Name.Equals(last, StringComparison.OrdinalIgnoreCase));
        if (found == null)
        {
            target.Add(last, value);
        }
        else
        {
            found.Value = value;
        }
    }
}

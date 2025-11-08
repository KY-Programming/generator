using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KY.Generator.Command;

[DebuggerDisplay("{Name,nq}={Value}")]
public class CliCommandParameter
{
    private static readonly Regex regex = new(@"^-\*?(?<name>[a-zA-Z0-9-]+)(=(?<value>.*))?$");

    public string Name { get; }
    public string Value { get; set; }

    public CliCommandParameter(string name, string value)
    {
        this.Name = name;
        this.Value = value;
    }

    public static CliCommandParameter Parse(string text)
    {
        Match match = regex.Match(text);
        if (!match.Success)
        {
            throw new InvalidOperationException($"Invalid parameter '{text}'");
        }
        string name = FormatName(match.Groups["name"].Value);
        string value = match.Groups["value"].Value;
        return new CliCommandParameter(name, value);
    }

    public static string FormatName(string name)
    {
        return name?.Replace("-", string.Empty)?.ToLowerInvariant();
    }

    public override string ToString()
    {
        string value = string.IsNullOrEmpty(this.Value) ? string.Empty : $"=\"{this.Value.TrimEnd('\\')}\"";
        return $"-{this.Name}{value}";
    }
}
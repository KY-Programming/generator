using System;
using System.Text.RegularExpressions;

namespace KY.Generator.Command
{
    public class CommandParameter
    {
        private static readonly Regex regex = new Regex(@"^-(?<name>[\w-]+)(=(?<value>.+))?$");
        public string Name { get; }
        public string Value { get; }

        public CommandParameter(string name)
        {
            this.Name = name;
        }

        public CommandParameter(string name, string value)
            : this(name)
        {
            this.Value = value;
        }

        public static CommandParameter Parse(string text)
        {
            Match match = regex.Match(text);
            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid parameter '{text}'");
            }
            string name = match.Groups["name"].Value;
            string value = match.Groups["value"].Value;
            return new CommandParameter(name, value);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Value) ? $"-{this.Name}" : $"-{this.Name}=\"{this.Value}\"";
        }
    }
}
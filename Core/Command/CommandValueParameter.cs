using System;
using System.Text.RegularExpressions;

namespace KY.Generator.Command
{
    public class CommandValueParameter : CommandParameter
    {
        private static readonly Regex regex = new Regex(@"^-(?<name>[\w-]+)(=(?<value>.+))?$");
        public string Value { get; }

        public CommandValueParameter(string name, string value)
            : base(name)
        {
            this.Value = value;
        }

        public static CommandValueParameter Parse(string text)
        {
            Match match = regex.Match(text);
            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid parameter '{text}'");
            }
            string name = match.Groups["name"].Value;
            string value = match.Groups["value"].Value;
            return new CommandValueParameter(name, value);
        }

        public override string ToString()
        {
            return $"-{this.Name}=\"{this.Value}\"";
        }
    }
}
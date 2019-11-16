using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Languages;

namespace KY.Generator.Command
{
    public class CommandConfiguration : ConfigurationBase
    {
        public override bool RequireLanguage => false;
        public string Command { get; }
        public List<CommandParameter> Parameters { get; }

        public CommandConfiguration(string command)
        {
            this.Command = command;
            this.Parameters = new List<CommandParameter>();
            this.Language = new ForwardFileLanguage();
        }
    }
}
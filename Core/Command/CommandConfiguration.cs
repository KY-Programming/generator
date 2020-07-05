using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configurations;
using KY.Generator.Languages;

namespace KY.Generator.Command
{
    public class CommandConfiguration : ConfigurationBase
    {
        public override bool RequireLanguage => false;
        public string Command { get; }
        public List<ICommandParameter> Parameters { get; }

        public CommandConfiguration(string command)
        {
            this.Command = command;
            this.Parameters = new List<ICommandParameter>();
            this.Language = new ForwardFileLanguage();
        }

        public void AddParameters(IEnumerable<string> parameters)
        {
            this.AddParameters(parameters.ToArray());
        }
        
        public void AddParameters(params string[] parameters)
        {
            this.Parameters.AddRange(CommandParameterParser.Parse(parameters));
        }
    }
}
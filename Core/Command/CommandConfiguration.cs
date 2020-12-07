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
        public bool SkipAsyncCheck { get; set; }
        public bool IsAsyncAssembly { get; set; }

        public CommandConfiguration(string command)
        {
            this.Command = command;
            this.Parameters = new List<ICommandParameter>();
            this.Language = new ForwardFileLanguage();
        }

        public CommandConfiguration AddParameters(IEnumerable<string> parameters)
        {
            this.AddParameters(parameters.ToArray());
            return this;
        }
        
        public CommandConfiguration AddParameters(params string[] parameters)
        {
            this.AddParameters(CommandParameterParser.Parse(parameters));
            return this;
        }

        public CommandConfiguration AddParameters(IEnumerable<ICommandParameter> parameters)
        {
            this.AddParameters(parameters.ToArray());
            return this;
        }
        
        public CommandConfiguration AddParameters(params ICommandParameter[] parameters)
        {
            this.Parameters.AddRange(parameters);
            return this;
        }
    }
}
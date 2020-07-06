using System.Collections.Generic;
using System.Linq;

namespace KY.Generator
{
    public class AttributeCommandConfiguration
    {
        public string Command { get; }
        public List<string> Parameters { get; }

        public AttributeCommandConfiguration(string command, IEnumerable<string> parameters)
        {
            this.Command = command;
            this.Parameters = parameters?.ToList() ?? new List<string>();
        }

        public AttributeCommandConfiguration(string command, params string[] parameters)
            : this(command, parameters.ToList())
        { }
    }
}
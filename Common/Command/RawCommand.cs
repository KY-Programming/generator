using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Command
{
    [DebuggerDisplay("{Name}*{Parameters.Count}")]
    public class RawCommand
    {
        public string Name { get; }
        public List<RawCommandParameter> Parameters { get; } = new List<RawCommandParameter>();

        public RawCommand(string name)
        {
            this.Name = name;
        }
    }
}

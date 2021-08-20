using System.Collections.Generic;

namespace KY.Generator.Command
{
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
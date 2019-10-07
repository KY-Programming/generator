namespace KY.Generator.Command
{
    public class CommandParameter
    {
        public string Name { get; }

        public CommandParameter(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
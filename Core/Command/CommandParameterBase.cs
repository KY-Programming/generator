namespace KY.Generator.Command
{
    public abstract class CommandParameterBase : ICommandParameter
    {
        public string Name { get; }

        protected CommandParameterBase(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
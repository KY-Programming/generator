namespace KY.Generator.Templates
{
    public class LocalVariableTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public string Name { get; }

        public LocalVariableTemplate(string name)
        {
            this.Name = name;
        }
    }
}
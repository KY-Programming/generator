namespace KY.Generator.Templates
{
    public class ExecutePropertyTemplate : ChainedCodeFragment
    {
        public override string Separator => ".";
        public string Name { get; set; }

        public ExecutePropertyTemplate(string name)
        {
            this.Name = name;
        }
    }
}
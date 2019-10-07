namespace KY.Generator.Templates
{
    public class ExecuteFieldTemplate : ChainedCodeFragment
    {
        public override string Separator => ".";
        public string Name { get; set; }

        public ExecuteFieldTemplate(string name)
        {
            this.Name = name;
        }
    }
}
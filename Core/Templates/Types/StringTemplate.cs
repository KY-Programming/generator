namespace KY.Generator.Templates
{
    public class StringTemplate : ICodeFragment
    {
        public string Value { get; }

        public StringTemplate(string value)
        {
            this.Value = value;
        }
    }
}
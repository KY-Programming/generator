namespace KY.Generator.Templates
{
    public class StringTemplate : CodeFragment
    {
        public string Value { get; }

        public StringTemplate(string value)
        {
            this.Value = value;
        }
    }
}
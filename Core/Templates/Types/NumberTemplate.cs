namespace KY.Generator.Templates
{
    public class NumberTemplate : ICodeFragment
    {
        public int Value { get; }

        public NumberTemplate(int value)
        {
            this.Value = value;
        }
    }
}
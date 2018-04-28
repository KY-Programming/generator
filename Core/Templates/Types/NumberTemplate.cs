namespace KY.Generator.Templates
{
    public class NumberTemplate : CodeFragment
    {
        public int Value { get; }

        public NumberTemplate(int value)
        {
            this.Value = value;
        }
    }
}
namespace KY.Generator.Templates
{
    public class ElseTemplate : CodeFragment
    {
        public MultilineCodeFragment Code { get; }

        public ElseTemplate()
        {
            this.Code = new MultilineCodeFragment();
        }
    }
}
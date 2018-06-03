namespace KY.Generator.Templates
{
    public class ElseTemplate : ICodeFragment
    {
        public MultilineCodeFragment Code { get; }

        public ElseTemplate()
        {
            this.Code = new MultilineCodeFragment();
        }
    }
}
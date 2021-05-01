namespace KY.Generator.Templates
{
    public class ElseTemplate : ICodeFragment
    {
        private readonly IfTemplate template;
        public MultilineCodeFragment Code { get; }

        public ElseTemplate(IfTemplate template)
        {
            this.template = template;
            this.Code = new MultilineCodeFragment();
        }

        public IfTemplate Close()
        {
            return this.template;
        }
    }
}
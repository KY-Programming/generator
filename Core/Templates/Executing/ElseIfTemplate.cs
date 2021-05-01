namespace KY.Generator.Templates
{
    public class ElseIfTemplate : ICodeFragment
    {
        public IfTemplate IfTemplate { get; }
        
        public ICodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; }

        public ElseIfTemplate(IfTemplate template, ICodeFragment condition)
        {
            this.IfTemplate = template;
            this.Condition = condition;
            this.Code = new MultilineCodeFragment();
        }

        public IfTemplate Close()
        {
            return this.IfTemplate;
        }
    }
}
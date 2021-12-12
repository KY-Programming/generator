namespace KY.Generator.Templates
{
    public class MethodGenericTemplate : ICodeFragment
    {
        public string Alias { get; set; }
        public TypeTemplate DefaultType { get; set; }

        public MethodGenericTemplate(string alias, TypeTemplate defaultType = null)
        {
            this.Alias = alias;
            this.DefaultType = defaultType;
        }
    }
}

namespace KY.Generator.Templates
{
    public class DeclareTemplate : ICodeFragment
    {
        public TypeTemplate Type { get; }
        public string Name { get; }
        public ICodeFragment Code { get; }

        public DeclareTemplate(TypeTemplate type, string name, ICodeFragment code)
        {
            this.Type = type;
            this.Name = name;
            this.Code = code;
        }
    }
}
namespace KY.Generator.Templates
{
    public class DeclareTemplate : CodeFragment
    {
        public TypeTemplate Type { get; }
        public string Name { get; }
        public CodeFragment Code { get; }

        public DeclareTemplate(TypeTemplate type, string name, CodeFragment code)
        {
            this.Type = type;
            this.Name = name;
            this.Code = code;
        }
    }
}
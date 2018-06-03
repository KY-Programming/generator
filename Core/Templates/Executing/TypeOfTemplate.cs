namespace KY.Generator.Templates
{
    public class TypeOfTemplate : ICodeFragment
    {
        public TypeTemplate Type { get; }

        public TypeOfTemplate(TypeTemplate type)
        {
            this.Type = type;
        }
    }
}
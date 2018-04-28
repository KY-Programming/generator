namespace KY.Generator.Templates
{
    public class TypeOfTemplate : CodeFragment
    {
        public TypeTemplate Type { get; }

        public TypeOfTemplate(TypeTemplate type)
        {
            this.Type = type;
        }
    }
}
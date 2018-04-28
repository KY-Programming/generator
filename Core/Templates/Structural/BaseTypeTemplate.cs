namespace KY.Generator.Templates
{
    public class BaseTypeTemplate : CodeFragment
    {
        private readonly TypeTemplate type;

        public BaseTypeTemplate(TypeTemplate type)
        {
            this.type = type;
        }

        public TypeTemplate ToType()
        {
            return this.type;
        }
    }
}
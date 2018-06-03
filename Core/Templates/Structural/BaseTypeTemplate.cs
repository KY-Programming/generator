namespace KY.Generator.Templates
{
    public class BaseTypeTemplate : ICodeFragment
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
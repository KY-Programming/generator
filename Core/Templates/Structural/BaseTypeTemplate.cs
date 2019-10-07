namespace KY.Generator.Templates
{
    public class BaseTypeTemplate : ICodeFragment
    {
        public ClassTemplate Parent { get; }
        private readonly TypeTemplate type;

        public BaseTypeTemplate(ClassTemplate parent, TypeTemplate type)
        {
            this.Parent = parent;
            this.type = type;
        }

        public TypeTemplate ToType()
        {
            return this.type;
        }
    }
}
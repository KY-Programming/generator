namespace KY.Generator.Templates
{
    public class CastTemplate : ICodeFragment
    {
        public TypeTemplate Type { get; }
        public ICodeFragment Code { get; }

        public CastTemplate(TypeTemplate type, ICodeFragment code)
        {
            this.Type = type;
            this.Code = code;
        }
    }
}
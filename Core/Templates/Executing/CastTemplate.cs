namespace KY.Generator.Templates
{
    public class CastTemplate : CodeFragment
    {
        public TypeTemplate Type { get; }
        public CodeFragment Code { get; }

        public CastTemplate(TypeTemplate type, CodeFragment code)
        {
            this.Type = type;
            this.Code = code;
        }
    }
}
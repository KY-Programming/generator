using KY.Generator.Templates;

namespace KY.Generator
{
    public static partial class Code
    {
        public static GenericTypeTemplate Generic(string name, params TypeTemplate[] types)
        {
            GenericTypeTemplate generic = new GenericTypeTemplate(name);
            generic.Types.AddRange(types);
            return generic;
        }

        public static TypeTemplate Type(string type, bool nullable = false)
        {
            return type == null ? null : new TypeTemplate(type, false, nullable);
        }

        public static TypeTemplate Interface(string type)
        {
            return type == null ? null : new TypeTemplate(type, true);
        }

        public static CommentTemplate Comment(string description)
        {
            return new CommentTemplate(description);
        }
    }
}
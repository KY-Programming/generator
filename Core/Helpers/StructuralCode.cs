using KY.Generator.Templates;

namespace KY.Generator
{
    public static class StructuralCode
    {
        public static GenericTypeTemplate Generic(this Code _, string name, params TypeTemplate[] types)
        {
            GenericTypeTemplate generic = new GenericTypeTemplate(name);
            generic.Types.AddRange(types);
            return generic;
        }

        public static GenericTypeTemplate Generic(this Code _, string name, string nameSpace = null, bool nullable = false, params TypeTemplate[] types)
        {
            GenericTypeTemplate generic = new GenericTypeTemplate(name, nameSpace, nullable);
            generic.Types.AddRange(types);
            return generic;
        }

        public static GenericTypeTemplate Generic(this Code _, string name, string nameSpace = null, bool nullable = false, bool fromSystem = false, params TypeTemplate[] types)
        {
            GenericTypeTemplate generic = new GenericTypeTemplate(name, nameSpace, nullable, fromSystem);
            generic.Types.AddRange(types);
            return generic;
        }

        public static TypeTemplate Type(this Code _, string type, string nameSpace = null, bool nullable = false, bool fromSystem = false)
        {
            return type == null ? null : new TypeTemplate(type, nameSpace, false, nullable, fromSystem);
        }

        public static TypeTemplate Interface(this Code _, string type, string nameSpace = null, bool fromSystem = false)
        {
            return type == null ? null : new TypeTemplate(type, nameSpace, true, false, fromSystem);
        }

        public static CommentTemplate Comment(this Code _, string description)
        {
            return new CommentTemplate(description);
        }
    }
}
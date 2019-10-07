using KY.Generator.Templates;

namespace KY.Generator
{
    public static class TypeCode
    {
        public static StringTemplate String(this Code _, string value)
        {
            return new StringTemplate(value);
        }

        public static NumberTemplate Number(this Code _, int value)
        {
            return new NumberTemplate(value);
        }

        public static VoidTemplate Void(this Code _)
        {
            return new VoidTemplate();
        }

        public static NullTemplate Null(this Code _)
        {
            return new NullTemplate();
        }

        public static AnonymousObjectTemplate AnonymousObject(this Code _)
        {
            return new AnonymousObjectTemplate();
        }
    }
}
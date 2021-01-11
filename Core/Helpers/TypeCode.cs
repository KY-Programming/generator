using System;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static class TypeCode
    {
        public static StringTemplate String(this Code _, string value)
        {
            return new StringTemplate(value);
        }

        public static NumberTemplate Number(this Code _, long value)
        {
            return new NumberTemplate(value);
        }

        public static NumberTemplate Number(this Code _, float value)
        {
            return new NumberTemplate(value);
        }

        public static NumberTemplate Number(this Code _, double value)
        {
            return new NumberTemplate(value);
        }

        public static DateTimeTemplate DateTime(this Code _, DateTime value)
        {
            return new DateTimeTemplate(value);
        }

        public static BooleanTemplate Boolean(this Code _, bool value)
        {
            return new BooleanTemplate(value);
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
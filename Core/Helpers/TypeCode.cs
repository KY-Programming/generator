using KY.Generator.Templates;

namespace KY.Generator
{
    public static partial class Code
    {
        public static StringTemplate String(string value)
        {
            return new StringTemplate(value);
        }

        public static NumberTemplate Number(int value)
        {
            return new NumberTemplate(value);
        }

        public static VoidTemplate Void()
        {
            return new VoidTemplate();
        }

        public static NullTemplate Null()
        {
            return new NullTemplate();
        }
    }
}
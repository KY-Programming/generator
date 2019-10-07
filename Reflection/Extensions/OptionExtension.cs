using KY.Generator.Reflection.Attributes;

namespace KY.Generator.Reflection.Extensions
{
    public static class OptionExtension
    {
        public static bool ToBool(this Option option, bool defaultValue)
        {
            return option == Option.Inherit ? defaultValue : option == Option.Yes;
        }
    }
}
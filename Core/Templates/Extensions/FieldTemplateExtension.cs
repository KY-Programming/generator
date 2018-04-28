using KY.Generator.Models;

namespace KY.Generator.Templates.Extensions
{
    public static class FieldTemplateExtension
    {
        public static FieldTemplate Public(this FieldTemplate field)
        {
            field.Visibility = Visibility.Public;
            return field;
        }

        public static FieldTemplate Protected(this FieldTemplate field)
        {
            field.Visibility = Visibility.Protected;
            return field;
        }

        public static FieldTemplate Static(this FieldTemplate field)
        {
            field.IsStatic = true;
            return field;
        }

        public static FieldTemplate Const(this FieldTemplate field)
        {
            field.IsConst = true;
            return field;
        }
    }
}
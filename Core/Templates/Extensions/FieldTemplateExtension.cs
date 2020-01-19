using KY.Generator.Configuration;
using KY.Generator.Languages;
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

        public static FieldTemplate Static(this FieldTemplate field, bool value = true)
        {
            field.IsStatic = value;
            return field;
        }

        public static FieldTemplate Const(this FieldTemplate field, bool value = true)
        {
            field.IsConst = value;
            return field;
        }

        public static FieldTemplate Readonly(this FieldTemplate field, bool value = true)
        {
            field.IsReadonly = value;
            return field;
        }

        public static FieldTemplate Default(this FieldTemplate field, ICodeFragment code)
        {
            field.DefaultValue = code;
            return field;
        }

        public static FieldTemplate FormatName(this FieldTemplate field, IConfiguration configuration)
        {
            field.Name = Formatter.FormatField(field.Name, configuration);
            return field;
        }

        public static FieldTemplate FormatName(this FieldTemplate field, ILanguage language, bool formatNames)
        {
            field.Name = Formatter.FormatField(field.Name, language, formatNames);
            return field;
        }
    }
}
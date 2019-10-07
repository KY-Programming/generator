using System;

namespace KY.Generator.Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAttribute : Attribute
    {
        public OutputLanguage Language { get; }
        public string RelativePath { get; }
        public Option SkipNamespace { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }

        public GenerateAttribute(OutputLanguage language = OutputLanguage.Inherit, string relativePath = null, Option skipNamespace = Option.Inherit, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
        {
            this.Language = language;
            this.RelativePath = relativePath;
            this.SkipNamespace = skipNamespace;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
        }
    }
}
using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenerateIndexAttribute : GenerateAttribute
    {
        public GenerateIndexAttribute(OutputLanguage language = OutputLanguage.Inherit, string relativePath = null, Option skipNamespace = Option.Inherit, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
            : base(language, relativePath, skipNamespace, propertiesToFields, fieldsToProperties, formatNames, Option.Yes)
        {
        }
    }
}
using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAngularModelAttribute : Attribute, IGeneratorCommandAttribute
    {
        public string Command { get; } = "angular-model";

        public List<string> Parameters
        {
            get
            {
                List<string> parameter = new List<string>();
                if (this.RelativePath != null)
                {
                    parameter.Add($"-relativePath={this.RelativePath}");
                }
                if (this.SkipNamespace == Option.Yes)
                {
                    parameter.Add("-skipNamespace");
                }
                if (this.PropertiesToFields == Option.Yes)
                {
                    parameter.Add("-propertiesToFields");
                }
                if (this.FieldsToProperties == Option.Yes)
                {
                    parameter.Add("-fieldsToProperties");
                }
                if (this.FormatNames == Option.Yes)
                {
                    parameter.Add("-formatNames");
                }
                return parameter;
            }
        }

        public string RelativePath { get; }
        public Option SkipNamespace { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }

        public GenerateAngularModelAttribute(string relativePath = null, Option skipNamespace = Option.Inherit, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
        {
            this.RelativePath = relativePath;
            this.SkipNamespace = skipNamespace;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
        }
    }
}
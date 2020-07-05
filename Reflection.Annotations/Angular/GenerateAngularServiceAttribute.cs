using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAngularServiceAttribute : Attribute, IGeneratorCommandAttribute
    {
        public string Command { get; } = "angular-service";
        public List<string> Parameters 
        {
            get
            {
                List<string> parameter = new List<string>();
                if (this.RelativePath != null)
                {
                    parameter.Add($"-relativePath={this.RelativePath}");
                }
                if (this.Name != null)
                {
                    parameter.Add($"-name={this.Name}");
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
        public string Name { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }

        public GenerateAngularServiceAttribute(string relativePath = null, string name = null, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
        {
            this.RelativePath = relativePath;
            this.Name = name;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
        }
    }
}
using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAngularServiceAttribute : Attribute, IGeneratorCommandAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get
            {
                return new[]
                       {
                           new AttributeCommandConfiguration("asp-read-controller", "-namespace=$NAMESPACE$", "-name=$NAME$"),
                           new AttributeCommandConfiguration("angular-service", this.ServiceParameters),
                           new AttributeCommandConfiguration("angular-model", this.ModelParameters)
                       };
            }
        }

        private List<string> ServiceParameters
        {
            get
            {
                List<string> parameter = new List<string>();
                if (this.RelativePath != null)
                {
                    parameter.Add($"-relativePath={this.RelativePath}");
                }
                if (this.RelativeModelPath != null)
                {
                    parameter.Add($"-relativeModelPath={this.RelativeModelPath}");
                }
                if (this.Name != null)
                {
                    parameter.Add($"-name={this.Name}");
                }
                if (this.PropertiesToFields == Option.Yes)
                {
                    parameter.Add("-propertiesToFields");
                }
                else if (this.PropertiesToFields == Option.No)
                {
                    parameter.Add("-propertiesToFields=false");
                }
                if (this.FieldsToProperties == Option.Yes)
                {
                    parameter.Add("-fieldsToProperties");
                }
                else if (this.FieldsToProperties == Option.No)
                {
                    parameter.Add("-fieldsToProperties=false");
                }
                if (this.FormatServiceNames == Option.Yes)
                {
                    parameter.Add("-formatNames");
                }
                else if (this.FormatServiceNames == Option.No)
                {
                    parameter.Add("-formatNames=false");
                }
                return parameter;
            }
        }

        private List<string> ModelParameters
        {
            get
            {
                List<string> parameter = new List<string>();
                if (this.RelativePath != null)
                {
                    parameter.Add($"-relativePath={this.RelativeModelPath}");
                }
                if (this.PropertiesToFields == Option.Yes)
                {
                    parameter.Add("-propertiesToFields");
                }
                else if (this.PropertiesToFields == Option.No)
                {
                    parameter.Add("-propertiesToFields=false");
                }
                if (this.FieldsToProperties == Option.Yes)
                {
                    parameter.Add("-fieldsToProperties");
                }
                else if (this.FieldsToProperties == Option.No)
                {
                    parameter.Add("-fieldsToProperties=false");
                }
                if (this.FormatModelNames == Option.Yes)
                {
                    parameter.Add("-formatNames");
                }
                else if (this.FormatModelNames == Option.No)
                {
                    parameter.Add("-formatNames=false");
                }
                return parameter;
            }
        }

        public string RelativePath { get; }
        public string RelativeModelPath { get; set; }
        public string Name { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatServiceNames { get; }
        public Option FormatModelNames { get; }

        public GenerateAngularServiceAttribute()
        { }

        public GenerateAngularServiceAttribute(string relativeServicePath, string relativeModelPath, string name = null, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit, Option formatServiceNames = Option.Inherit, Option formatModelNames = Option.Inherit)
        {
            this.RelativePath = relativeServicePath;
            this.RelativeModelPath = relativeModelPath;
            this.Name = name;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatServiceNames = formatNames == Option.Inherit ? formatServiceNames : formatNames;
            this.FormatModelNames = formatNames == Option.Inherit ? formatModelNames : formatNames;
        }
    }
}
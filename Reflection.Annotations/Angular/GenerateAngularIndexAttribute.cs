using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAngularIndexAttribute : Attribute, IGeneratorCommandAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get { return new[]
                         {
                             new AttributeCommandConfiguration("reflection-read", "-namespace=$NAMESPACE$", "-name=$NAME$", "-skipSelf"),
                             new AttributeCommandConfiguration("angular-model", this.Parameters)
                         }; }
        }

        private List<string> Parameters
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
                else if (this.SkipNamespace == Option.No)
                {
                    parameter.Add("-skipNamespace=false");
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
                if (this.FormatNames == Option.Yes)
                {
                    parameter.Add("-formatNames");
                }
                else if (this.FormatNames == Option.No)
                {
                    parameter.Add("-formatNames=false");
                }
                return parameter;
            }
        }

        public string RelativePath { get; }
        public Option SkipNamespace { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }

        public GenerateAngularIndexAttribute(string relativePath = null, Option skipNamespace = Option.Inherit, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
        {
            this.RelativePath = relativePath;
            this.SkipNamespace = skipNamespace;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
        }
    }
}
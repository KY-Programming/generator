using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAttribute : Attribute, IGeneratorCommandAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get { return new[] { new AttributeCommandConfiguration("reflection", this.Parameters) }; }
        }

        private IEnumerable<string> Parameters
        {
            get
            {
                List<string> parameter = new List<string>
                                         {
                                             "-namespace=$NAMESPACE$", 
                                             "-name=$NAME$"
                                         };
                if (this.Language != OutputLanguage.Inherit)
                {
                    parameter.Add($"-language={this.Language}");
                }
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
                if (this.SkipSelf == Option.Yes)
                {
                    parameter.Add("-skipSelf");
                }
                else if (this.SkipSelf == Option.No)
                {
                    parameter.Add("-skipSelf=false");
                }
                return parameter;
            }
        }

        public OutputLanguage Language { get; }
        public string RelativePath { get; }
        public Option SkipNamespace { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }
        public Option SkipSelf { get; }

        public GenerateAttribute(OutputLanguage language = OutputLanguage.Inherit, string relativePath = null, Option skipNamespace = Option.Inherit, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit, Option skipSelf = Option.Inherit)
        {
            this.Language = language;
            this.RelativePath = relativePath;
            this.SkipNamespace = skipNamespace;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
            this.SkipSelf = skipSelf;
        }
    }
}
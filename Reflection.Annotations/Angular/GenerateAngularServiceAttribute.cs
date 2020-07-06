﻿using System;
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
                           new AttributeCommandConfiguration("angular-service", this.Parameters)
                       };
            }
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
        public string RelativeModelPath { get; set; }
        public string Name { get; }
        public Option PropertiesToFields { get; }
        public Option FieldsToProperties { get; }
        public Option FormatNames { get; }

        public GenerateAngularServiceAttribute()
        { }

        public GenerateAngularServiceAttribute(string relativeServicePath, string relativeModelPath, string name = null, Option propertiesToFields = Option.Inherit, Option fieldsToProperties = Option.Inherit, Option formatNames = Option.Inherit)
        {
            this.RelativePath = relativeServicePath;
            this.RelativeModelPath = relativeModelPath;
            this.Name = name;
            this.PropertiesToFields = propertiesToFields;
            this.FieldsToProperties = fieldsToProperties;
            this.FormatNames = formatNames;
        }
    }
}
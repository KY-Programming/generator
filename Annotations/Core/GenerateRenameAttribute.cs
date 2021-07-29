using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateRenameAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
    {
        public string Replace { get; }
        public string With { get; }

        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get
            {
                return new[]
                       {
                           new AttributeCommandConfiguration("asp-read-controller", this.Parameters)
                       };
            }
        }

        private List<string> Parameters
        {
            get
            {
                List<string> parameter = new List<string>();
                parameter.Add($"-replace-name={this.Replace}");
                parameter.Add($"-replace-with-name={this.With}");
                return parameter;
            }
        }

        public GenerateRenameAttribute(string replace, string with = null)
        {
            this.Replace = replace;
            this.With = with ?? string.Empty;
        }
    }
}

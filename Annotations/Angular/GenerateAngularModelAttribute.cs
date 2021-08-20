using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateAngularModelAttribute : Attribute, IGeneratorCommandAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get { return new[]
                         {
                             new AttributeCommandConfiguration("reflection-read", "-namespace=$NAMESPACE$", "-name=$NAME$"),
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
                return parameter;
            }
        }

        public string RelativePath { get; }

        public GenerateAngularModelAttribute(string relativePath = null)
        {
            this.RelativePath = relativePath;
        }
    }
}

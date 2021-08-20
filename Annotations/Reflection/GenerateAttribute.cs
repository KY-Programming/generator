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
                if (this.SkipSelf)
                {
                    parameter.Add("-skipSelf");
                }
                return parameter;
            }
        }

        public OutputLanguage Language { get; }
        public string RelativePath { get; }
        public bool SkipSelf { get; }

        public GenerateAttribute(OutputLanguage language = OutputLanguage.Inherit, string relativePath = null, bool skipSelf = false)
        {
            this.Language = language;
            this.RelativePath = relativePath;
            this.SkipSelf = skipSelf;
        }
    }
}

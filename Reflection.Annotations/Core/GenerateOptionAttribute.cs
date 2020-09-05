using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateOptionAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
    {
        private readonly string[] options;

        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get
            {
                return new[]
                       {
                           new AttributeCommandConfiguration("*", this.options)
                       };
            }
        }

        public GenerateOptionAttribute(params string[] options)
        {
            this.options = options;
        }
    }
}
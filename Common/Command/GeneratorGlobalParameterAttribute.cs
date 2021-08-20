using System;

namespace KY.Generator.Command
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class GeneratorGlobalParameterAttribute : Attribute
    {
        public string ParameterName { get; }

        public GeneratorGlobalParameterAttribute(string parameterName = null)
        {
            this.ParameterName = parameterName;
        }
    }
}
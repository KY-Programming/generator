using System;

namespace KY.Generator.Command
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class GeneratorParameterAttribute : Attribute
    {
        public string ParameterName { get; }

        public GeneratorParameterAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }
    }
}
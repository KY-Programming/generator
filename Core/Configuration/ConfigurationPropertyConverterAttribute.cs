using System;

namespace KY.Generator.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationPropertyConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public ConfigurationPropertyConverterAttribute(Type converterType)
        {
            this.ConverterType = converterType;
        }
    }
}
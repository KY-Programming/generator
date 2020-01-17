using System;

namespace KY.Generator.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConfigurationPropertyAttribute : Attribute
    {
        public string Alias { get; }

        public ConfigurationPropertyAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
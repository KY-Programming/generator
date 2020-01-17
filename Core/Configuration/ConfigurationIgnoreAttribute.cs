using System;

namespace KY.Generator.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationIgnoreAttribute : Attribute
    {
    }
}
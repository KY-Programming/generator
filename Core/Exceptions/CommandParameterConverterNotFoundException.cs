using System;
using KY.Generator.Configuration;

namespace KY.Generator.Exceptions
{
    public class CommandParameterConverterNotFoundException : Exception
    {
        public CommandParameterConverterNotFoundException(Type type)
            : base($"Converter for '{type.FullName} not found. Use [{typeof(ConfigurationPropertyConverterAttribute).Name.Replace("Attribute", string.Empty)}(typeof(MyConverter)] and derive your converter from {typeof(ConfigurationPropertyConverter)}.")
        { }
    }
}
using System;

namespace KY.Generator.Exceptions
{
    public class ConfigurationParameterReadOnlyException : Exception
    {
        public ConfigurationParameterReadOnlyException(string parameter)
            : base($"Parameter '{parameter} is readonly. Add a setter (set;).")
        { }
    }
}
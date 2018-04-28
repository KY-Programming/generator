using System;

namespace KY.Generator.Configuration
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
        { }

        public InvalidConfigurationException(string message)
            : base(message)
        { }
    }
}
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

        public InvalidConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
using System;

namespace KY.Generator.Exceptions
{
    public class AmbiguousConfigurationException : Exception
    {
        public AmbiguousConfigurationException(Type configuration, string name, string group)
            : base($"Configuration '{configuration.Name}' is already used by {name} ({group}). Each configuration can only be used by one command. Create a new configuration class and derive from {configuration.Name}")
        { }
    }
}
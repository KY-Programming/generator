using System;
using KY.Generator.Configuration;

namespace KY.Generator.Exceptions
{
    public class CommandWithoutEnvironmentException : Exception
    {
        public CommandWithoutEnvironmentException(Type type)
            : base($"Command '{type.Name}' has no environment set. Ensure you set the 'Environment' property in constructor or derive from {nameof(ConfigurationBase)}")
        { }
    }
}
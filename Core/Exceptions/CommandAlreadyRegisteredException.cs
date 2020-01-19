using System;

namespace KY.Generator.Exceptions
{
    public class CommandAlreadyRegisteredException : Exception
    {
        public CommandAlreadyRegisteredException(string name, string group)
            : base($"{name} ({group}) is already registered. Please use a prefix like my-{name}")
        { }
    }
}
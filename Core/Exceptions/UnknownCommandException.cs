using System;

namespace KY.Generator.Exceptions
{
    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(string command)
            : base($"Unknown command '{command}'. Ensure the module is loaded")
        { }
    }
}
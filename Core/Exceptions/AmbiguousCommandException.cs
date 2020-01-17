using System;

namespace KY.Generator.Exceptions
{
    public class AmbiguousCommandException : Exception
    {
        public AmbiguousCommandException(string message = null, Exception innerException = null)
            : base(message, innerException)
        { }
    }
}
using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ProducesAttribute : Attribute
    {
        public Type Type { get; }
        public int StatusCode { get; }

        public ProducesAttribute(Type type, int statusCode = 200)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.StatusCode = statusCode;
        }
    }
}

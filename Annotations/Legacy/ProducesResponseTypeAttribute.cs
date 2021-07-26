using System;

namespace KY.Generator.Legacy
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ProducesResponseTypeAttribute : Attribute
    {
        public Type Type { get; }
        public int StatusCode { get; }

        public ProducesResponseTypeAttribute(Type type, int statusCode = 200)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.StatusCode = statusCode;
        }
    }
}

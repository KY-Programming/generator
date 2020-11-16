using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateIgnoreGenericAttribute : Attribute
    {
        public Type Type { get; }

        public GenerateIgnoreGenericAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
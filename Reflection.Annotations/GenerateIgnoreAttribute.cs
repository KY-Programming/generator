using System;

namespace KY.Generator.Reflection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class GenerateIgnoreAttribute : Attribute
    { }
}
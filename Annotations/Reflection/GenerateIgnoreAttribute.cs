using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public class GenerateIgnoreAttribute : Attribute
    { }
}
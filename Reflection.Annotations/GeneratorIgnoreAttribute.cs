using System;

namespace KY.Generator.Reflection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GeneratorIgnoreAttribute : Attribute
    { }
}
using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenerateOnlySubTypesAttribute : Attribute
    { }
}

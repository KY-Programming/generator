using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GenerateAsPrimaryKeyAttribute : Attribute
    { }
}

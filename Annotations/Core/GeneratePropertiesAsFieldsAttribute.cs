using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    public class GeneratePropertiesAsFieldsAttribute : Attribute
    {
    }
}

using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    public class GenerateWithoutHeaderAttribute : Attribute
    {
    }
}
using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    public class GenerateFormatNamesAttribute : Attribute
    {
        public bool FormatNames { get; }

        public GenerateFormatNamesAttribute(bool formatNames = true)
        {
            this.FormatNames = formatNames;
        }
    }
}
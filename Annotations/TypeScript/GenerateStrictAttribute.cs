using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    public class GenerateStrictAttribute : Attribute
    {
        public bool Strict { get; }

        public GenerateStrictAttribute(bool strict = true)
        {
            this.Strict = strict;
        }
    }
}

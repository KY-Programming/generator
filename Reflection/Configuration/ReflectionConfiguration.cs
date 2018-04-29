using System.Collections.Generic;
using KY.Generator.Configuration;

namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionConfiguration : ConfigurationBase
    {
        public List<ReflectionType> Types { get; }

        public ReflectionConfiguration()
        {
            this.Types = new List<ReflectionType>();
        }
    }
}
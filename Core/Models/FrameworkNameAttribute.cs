using System;

namespace KY.Generator.Models
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FrameworkNameAttribute : Attribute
    {
        public string Name { get; }

        public FrameworkNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
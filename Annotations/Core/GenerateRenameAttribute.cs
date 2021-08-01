using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateRenameAttribute : Attribute
    {
        public string Replace { get; }
        public string With { get; }

        public GenerateRenameAttribute(string replace, string with = null)
        {
            this.Replace = replace;
            this.With = with ?? string.Empty;
        }
    }
}

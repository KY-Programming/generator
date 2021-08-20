using System;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenerateIndexAttribute : GenerateAttribute
    {
        public GenerateIndexAttribute(OutputLanguage language = OutputLanguage.Inherit, string relativePath = null)
            : base(language, relativePath, true)
        {
        }
    }
}

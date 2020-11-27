using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public class GenerateAsyncAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands { get; } = new[] { new AttributeCommandConfiguration("*", "-async") };
    }
}
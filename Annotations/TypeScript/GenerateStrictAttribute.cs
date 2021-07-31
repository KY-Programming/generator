using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
    public class GenerateStrictAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands { get; } = new[]
                                                                              {
                                                                                  new AttributeCommandConfiguration("angular-service", "-strict"),
                                                                                  new AttributeCommandConfiguration("angular-model", "-strict")
                                                                              };
    }
}

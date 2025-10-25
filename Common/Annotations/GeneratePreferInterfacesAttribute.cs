using System;
using System.Collections.Generic;

namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GeneratePreferInterfacesAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new AttributeCommandConfiguration("angular-model", "-prefer-interfaces")
    ];
}

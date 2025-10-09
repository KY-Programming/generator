using System;
using System.Collections.Generic;

namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateServiceOutputAttribute(string relativePath) : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public string RelativePath { get; } = relativePath;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new AttributeCommandConfiguration("angular-model", this.Parameters),
        new AttributeCommandConfiguration("angular-service", this.Parameters)
    ];

    private List<string> Parameters
    {
        get
        {
            List<string> parameter = [];
            if (this.RelativePath != null)
            {
                parameter.Add($"-relativePath={this.RelativePath}");
            }
            return parameter;
        }
    }
}

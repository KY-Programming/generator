using System;
using System.Collections.Generic;

namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class GenerateAngularServiceAttribute : Attribute, IGeneratorCommandAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands
    {
        get
        {
            return new[]
                   {
                       new AttributeCommandConfiguration("asp-read-controller", "-namespace=$NAMESPACE$", "-name=$NAME$"),
                       new AttributeCommandConfiguration("angular-model", this.ModelParameters),
                       new AttributeCommandConfiguration("angular-service", this.ServiceParameters)
                   };
        }
    }

    private List<string> ServiceParameters
    {
        get
        {
            List<string> parameter = [];
            if (this.RelativePath != null)
            {
                parameter.Add($"-relativePath={this.RelativePath}");
            }
            if (this.RelativeModelPath != null)
            {
                parameter.Add($"-relativeModelPath={this.RelativeModelPath}");
            }
            if (this.Name != null)
            {
                parameter.Add($"-name={this.Name}");
            }
            return parameter;
        }
    }

    private List<string> ModelParameters
    {
        get
        {
            List<string> parameter = [];
            if (this.RelativePath != null)
            {
                parameter.Add($"-relativePath={this.RelativeModelPath}");
            }
            return parameter;
        }
    }

    public string RelativePath { get; }
    public string RelativeModelPath { get; set; }
    public string Name { get; }

    public GenerateAngularServiceAttribute()
    { }

    public GenerateAngularServiceAttribute(string relativeServicePath = null, string relativeModelPath = null, string name = null)
    {
        this.RelativePath = relativeServicePath;
        this.RelativeModelPath = relativeModelPath;
        this.Name = name;
    }
}

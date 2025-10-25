using System;

namespace KY.Generator;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public class GenerateReturnTypeAttribute : Attribute
{
    public string FileName { get; }
    public Type Type { get; }
    public string TypeName { get; set; }
    public string OverrideName { get; set; }

    /// <summary>
    /// Changes the return type
    /// </summary>
    public GenerateReturnTypeAttribute(Type type)
    {
        this.Type = type;
    }

    /// <summary>
    /// Changes the return type with an custom import
    /// </summary>
    /// <param name="type">Type used for the return of the property/method</param>
    /// <param name="fileName">File used for the using/import</param>
    /// <param name="importType">Optional import type. Overrides the type on the using/import</param>
    public GenerateReturnTypeAttribute(string type, string fileName, string importType = null)
    {
        this.FileName = fileName;
        this.TypeName = type;
        this.OverrideName = importType;
    }
}

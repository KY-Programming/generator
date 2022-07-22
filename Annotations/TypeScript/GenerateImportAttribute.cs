using System;

namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateImportAttribute : Attribute
{
    public Type Type { get; }
    public string FileName { get; }
    public string TypeName { get; }

    public GenerateImportAttribute(Type type, string fileName, string typeName)
    {
        this.Type = type;
        this.FileName = fileName;
        this.TypeName = typeName;
    }
}

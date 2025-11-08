using System;

namespace KY.Generator.Models;

public class Import
{
    public Type Type { get; }
    public string FileName { get; }
    public string TypeName { get; }

    public Import(Type type, string fileName, string typeName)
    {
        this.Type = type;
        this.FileName = fileName;
        this.TypeName = typeName;
    }
}

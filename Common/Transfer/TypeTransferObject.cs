using System.Diagnostics;
using KY.Generator.Templates;

namespace KY.Generator.Transfer;

[DebuggerDisplay("TypeTransferObject {Namespace,nq}.{Name,nq}")]
public class TypeTransferObject : ITransferObject
{
    private string originalName;

    public virtual string Name { get; set; }
    public virtual string FileName { get; set; }
    public virtual string OverrideType { get; set; }

    public virtual string OriginalName
    {
        get => this.originalName ?? this.Name;
        set => this.originalName = value;
    }

    public virtual string? Namespace { get; set; }
    public virtual bool FromSystem { get; set; }
    public virtual bool IsNullable { get; set; }
    public virtual bool IsGeneric { get; set; }
    public virtual bool IsGenericParameter { get; set; }
    public virtual bool IsInterface { get; set; }
    public virtual List<GenericAliasTransferObject> Generics { get; } = new();
    public virtual TypeTransferObject Original { get; set; }
    public virtual ICodeFragment Default { get; set; }
    public string FullName => $"{this.Namespace}.{this.Name}";

    public TypeTransferObject()
    { }

    public TypeTransferObject(TypeTransferObject type)
    {
        this.Name = type.Name;
        this.Namespace = type.Namespace;
        this.FromSystem = type.FromSystem;
        this.IsNullable = type.IsNullable;
        this.IsGeneric = type.IsGeneric;
        this.IsGenericParameter = type.IsGenericParameter;
        this.Generics = type.Generics.ToList();
    }

    public bool Equals(TypeTransferObject type)
    {
        return (this.Name == type.Name || this.OriginalName == type.Name || this.Name == type.OriginalName) && this.Namespace == type.Namespace && this.IsGeneric == type.IsGeneric;
    }
}

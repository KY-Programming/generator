namespace KY.Generator.Transfer;

public abstract class MemberTransferObject
{
    public string? Name { get; set; }
    public TypeTransferObject? Type { get; set; }
    public TypeTransferObject? DeclaringType { get; set; }
    public string? Comment { get; set; }
    public object? Default { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsOverwrite { get; set; }
    public bool IsOptional { get; set; } = true;
    public bool IsRequired { get; set; }
    public bool IsNullable { get; set; }
}

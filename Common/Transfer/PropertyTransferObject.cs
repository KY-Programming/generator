using KY.Core;

namespace KY.Generator.Transfer;

public class PropertyTransferObject : MemberTransferObject
{
    public bool CanRead { get; set; } = true;
    public bool CanWrite { get; set; } = true;
    public List<AttributeTransferObject> Attributes { get; set; } = [];

    public PropertyTransferObject()
    { }

    public PropertyTransferObject(PropertyTransferObject property)
    {
        this.Name = property.Name;
        this.Type = property.Type.Clone();
        this.DeclaringType = property.DeclaringType;
        this.CanRead = property.CanRead;
        this.CanRead = property.CanWrite;
        this.Attributes = property.Attributes.Select(attribute => attribute.Clone()).ToList();
        this.Comment = property.Comment;
    }
}

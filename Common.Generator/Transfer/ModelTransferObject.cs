using System.Diagnostics;
using KY.Core;
using KY.Generator.Languages;

namespace KY.Generator.Transfer;

[DebuggerDisplay("ModelTransferObject {Namespace,nq}.{Name,nq}")]
public class ModelTransferObject : TypeTransferObject
{
    public virtual bool IsEnum { get; set; }
    public virtual bool IsAbstract { get; set; }
    public virtual Dictionary<string, object>? EnumValues { get; set; }
    public virtual ModelTransferObject? BasedOn { get; set; }

    // TODO: Transfer objects should be independent from language
    [NotCloneable]
    public virtual ILanguage? Language { get; set; }

    public virtual List<TypeTransferObject> Interfaces { get; } = [];
    public virtual List<FieldTransferObject> Constants { get; } = [];
    public virtual List<FieldTransferObject> Fields { get; } = [];
    public virtual List<PropertyTransferObject> Properties { get; } = [];
    public virtual List<string> Usings { get; } = [];
    public virtual string? Comment { get; set; }

    [NotCloneable]
    public virtual Type Type { get; set; }

    public ModelTransferObject()
    { }

    public ModelTransferObject(TypeTransferObject type)
        : base(type)
    { }

    public ModelTransferObject(ModelTransferObject model)
        : base(model)
    {
        this.IsEnum = model.IsEnum;
        this.IsAbstract = model.IsAbstract;
        this.EnumValues = model.EnumValues?.ToDictionary(x => x.Key, x => x.Value);
        this.BasedOn = model.BasedOn;
        this.Language = model.Language;
        this.Interfaces = model.Interfaces.ToList();
        this.Constants = model.Constants.Select(x => new FieldTransferObject(x)).ToList();
        this.Fields = model.Fields.Select(x => new FieldTransferObject(x)).ToList();
        this.Properties = model.Properties.Select(x => new PropertyTransferObject(x)).ToList();
        this.Usings = model.Usings.ToList();
        this.Comment = model.Comment;
        this.Type = model.Type;
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KY.Generator.Templates;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("TypeTransferObject {Namespace,nq}.{Name,nq}")]
    public class TypeTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Namespace { get; set; }
        public bool FromSystem { get; set; }
        public bool IsNullable { get; set; }
        public List<GenericAliasTransferObject> Generics { get; }
        public TypeTransferObject Original { get; set; }
        public ICodeFragment Default { get; set; }
        public string FullName => $"{this.Namespace}.{this.Name}";
        public bool HasUsing { get; set; } = true;

        public TypeTransferObject()
        {
            this.Generics = new List<GenericAliasTransferObject>();
        }

        public TypeTransferObject(TypeTransferObject type)
        {
            this.Name = type.Name;
            this.Namespace = type.Namespace;
            this.FromSystem = type.FromSystem;
            this.IsNullable = type.IsNullable;
            this.Generics = type.Generics.ToList();
            this.HasUsing = type.HasUsing;
        }

        public bool Equals(TypeTransferObject type)
        {
            return (this.Name == type.Name || this.OriginalName == type.Name || this.Name == type.OriginalName) && this.Namespace == type.Namespace;
        }

        public TypeTransferObject Clone()
        {
            return (TypeTransferObject)this.MemberwiseClone();
        }
    }
}

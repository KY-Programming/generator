using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KY.Generator.Templates;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("TypeTransferObject {Namespace,nq}.{Name,nq}")]
    public class TypeTransferObject : ITransferObject
    {
        public virtual string Name { get; set; }
        public virtual string OriginalName { get; set; }
        public virtual string Namespace { get; set; }
        public virtual bool FromSystem { get; set; }
        public virtual bool IsNullable { get; set; }
        public virtual bool IsGeneric { get; set; }
        public virtual bool IsInterface { get; set; }
        public virtual List<GenericAliasTransferObject> Generics { get; }
        public virtual TypeTransferObject Original { get; set; }
        public virtual ICodeFragment Default { get; set; }
        public string FullName => $"{this.Namespace}.{this.Name}";
        public virtual bool HasUsing { get; set; } = true;

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
            this.IsGeneric = type.IsGeneric;
            this.Generics = type.Generics.ToList();
            this.HasUsing = type.HasUsing;
        }

        public bool Equals(TypeTransferObject type)
        {
            return (this.Name == type.Name || this.OriginalName == type.Name || this.Name == type.OriginalName) && this.Namespace == type.Namespace && this.IsGeneric == type.IsGeneric;
        }

        public TypeTransferObject Clone()
        {
            return (TypeTransferObject)this.MemberwiseClone();
        }
    }
}

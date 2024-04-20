using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KY.Core;
using KY.Generator.Templates;

namespace KY.Generator.Transfer
{
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

        public virtual string Namespace { get; set; }
        public virtual bool FromSystem { get; set; }
        public virtual bool IsNullable { get; set; }
        public virtual bool IsGeneric { get; set; }
        public virtual bool IsGenericParameter { get; set; }
        public virtual bool IsInterface { get; set; }
        public virtual List<GenericAliasTransferObject> Generics { get; } = [];
        public virtual TypeTransferObject Original { get; set; }
        public virtual ICodeFragment Default { get; set; }
        public string FullName => $"{this.Namespace}.{this.Name}";

        [NotCloneable]
        public virtual Type Type { get; set; }

        public TypeTransferObject()
        { }

        public TypeTransferObject(TypeTransferObject type)
        {
            this.Name = type.Name;
            this.FileName = type.FileName;
            this.OverrideType = type.OverrideType;
            this.OriginalName = type.OriginalName;
            this.Namespace = type.Namespace;
            this.FromSystem = type.FromSystem;
            this.IsNullable = type.IsNullable;
            this.IsGeneric = type.IsGeneric;
            this.IsGenericParameter = type.IsGenericParameter;
            this.IsInterface = type.IsInterface;
            this.Generics = type.Generics.ToList();
            this.Original = type.Original;
            this.Default = type.Default;
        }

        public bool Equals(TypeTransferObject type)
        {
            return (this.Name == type.Name || this.OriginalName == type.Name || this.Name == type.OriginalName) && this.Namespace == type.Namespace && this.IsGeneric == type.IsGeneric;
        }
    }
}

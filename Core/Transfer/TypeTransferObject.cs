using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("TypeTransferObject {Namespace,nq}.{Name,nq}")]
    public class TypeTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool FromSystem { get; set; }
        public bool IsNullable { get; set; }
        public List<TypeTransferObject> Generics { get; }

        public TypeTransferObject()
        {
            this.Generics = new List<TypeTransferObject>();
        }

        public bool Equals(TypeTransferObject type)
        {
            return this.Name == type.Name && this.Namespace == type.Namespace;
        }

        public TypeTransferObject Clone()
        {
            return (TypeTransferObject)this.MemberwiseClone();
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("ModelTransferObject {Namespace,nq}.{Name,nq}")]
    public class ModelTransferObject : TypeTransferObject
    {
        public bool IsEnum { get; set; }
        public bool IsInterface { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsGeneric { get; set; }
        public Dictionary<string, int> EnumValues { get; set; }
        public ModelTransferObject BasedOn { get; set; }
        public ILanguage Language { get; set; }
        public List<TypeTransferObject> Interfaces { get; }
        public List<ModelFieldTransferObject> Fields { get; }
        public List<ModelPropertyTransferObject> Properties { get; }

        public ModelTransferObject()
        {
            this.Interfaces = new List<TypeTransferObject>();
            this.Fields = new List<ModelFieldTransferObject>();
            this.Properties = new List<ModelPropertyTransferObject>();
        }
    }

    public class ModelFieldTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
    }

    public class ModelPropertyTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;
    }
    
    [DebuggerDisplay("TypeTransferObject {Namespace,nq}.{Name,nq}")]
    public class TypeTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool FromSystem { get; set; }
        public List<TypeTransferObject> Generics { get; }

        public TypeTransferObject()
        {
            this.Generics = new List<TypeTransferObject>();
        }

        public bool Equals(TypeTransferObject type)
        {
            return this.Name == type.Name && this.Namespace == type.Namespace;
        }
    }
}
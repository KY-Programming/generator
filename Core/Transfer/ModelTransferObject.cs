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
        public List<FieldTransferObject> Constants { get; }
        public List<FieldTransferObject> Fields { get; }
        public List<PropertyTransferObject> Properties { get; }
        public List<MethodTransferObject> Methods { get; }
        public List<string> Usings { get; }
        public string Comment { get; set; }

        public ModelTransferObject()
        {
            this.Interfaces = new List<TypeTransferObject>();
            this.Constants = new List<FieldTransferObject>();
            this.Fields = new List<FieldTransferObject>();
            this.Properties = new List<PropertyTransferObject>();
            this.Usings = new List<string>();
            this.Methods = new List<MethodTransferObject>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using KY.Core;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("ModelTransferObject {Namespace,nq}.{Name,nq}")]
    public class ModelTransferObject : TypeTransferObject, ICloneable
    {
        public virtual bool IsEnum { get; set; }
        public virtual bool IsAbstract { get; set; }
        public virtual Dictionary<string, int> EnumValues { get; set; }
        public virtual ModelTransferObject BasedOn { get; set; }
        public virtual ILanguage Language { get; set; }
        public virtual List<TypeTransferObject> Interfaces { get; }
        public virtual List<FieldTransferObject> Constants { get; }
        public virtual List<FieldTransferObject> Fields { get; }
        public virtual List<PropertyTransferObject> Properties { get; }
        public virtual List<MethodTransferObject> Methods { get; }
        public virtual List<string> Usings { get; }
        public virtual string Comment { get; set; }

        public ModelTransferObject()
        {
            this.Interfaces = new List<TypeTransferObject>();
            this.Constants = new List<FieldTransferObject>();
            this.Fields = new List<FieldTransferObject>();
            this.Properties = new List<PropertyTransferObject>();
            this.Usings = new List<string>();
            this.Methods = new List<MethodTransferObject>();
        }

        object ICloneable.Clone()
        {
            ModelTransferObject clone = this.Clone(false, nameof(this.Language));
            clone.Language = this.Language;
            return clone;
        }
    }
}

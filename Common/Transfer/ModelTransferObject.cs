using System;
using System.Collections.Generic;
using System.Diagnostics;
using KY.Core;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    [DebuggerDisplay("ModelTransferObject {Namespace,nq}.{Name,nq}")]
    public class ModelTransferObject : TypeTransferObject
    {
        public virtual bool IsEnum { get; set; }
        public virtual bool IsAbstract { get; set; }
        public virtual Dictionary<string, object> EnumValues { get; set; }
        public virtual ModelTransferObject BasedOn { get; set; }

        [NotCloneable]
        public virtual ILanguage Language { get; set; }

        public virtual List<TypeTransferObject> Interfaces { get; } = new();
        public virtual List<FieldTransferObject> Constants { get; } = new();
        public virtual List<FieldTransferObject> Fields { get; } = new();
        public virtual List<PropertyTransferObject> Properties { get; } = new();
        public virtual List<MethodTransferObject> Methods { get; } = new();
        public virtual List<string> Usings { get; } = new();
        public virtual string Comment { get; set; }

        [NotCloneable]
        public virtual Type Type { get; set; }
    }
}

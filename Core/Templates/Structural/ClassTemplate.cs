using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Class {Name}")]
    public class ClassTemplate : AttributeableTempalte, INamespaceChildren
    {
        public string Name { get; }
        public BaseTypeTemplate BasedOn { get; }
        public NamespaceTemplate Namespace { get; }
        public ClassTemplate ParentClass { get; }
        public List<ClassGenericTemplate> Generics { get; }
        public List<UsingTemplate> Usings { get; }
        public List<FieldTemplate> Fields { get; }
        public List<PropertyTemplate> Properties { get; }
        public List<MethodTemplate> Methods { get; }
        public List<ClassTemplate> Classes { get; }
        public ICodeFragment Code { get; set; }
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsInterface { get; set; }
        public CommentTemplate Comment { get; set; }

        private ClassTemplate(string name, TypeTemplate basedOn = null)
        {
            this.Name = name;
            this.BasedOn = basedOn == null ? null : new BaseTypeTemplate(basedOn);
            this.Generics = new List<ClassGenericTemplate>();
            this.Usings = new List<UsingTemplate>();
            this.Fields = new List<FieldTemplate>();
            this.Properties = new List<PropertyTemplate>();
            this.Methods = new List<MethodTemplate>();
            this.Classes = new List<ClassTemplate>();
        }

        public ClassTemplate(ClassTemplate parent, string name, TypeTemplate basedOn = null)
            : this(name, basedOn)
        {
            this.ParentClass = parent;
            this.Namespace = parent.Namespace;
        }

        public ClassTemplate(NamespaceTemplate parent, string name, TypeTemplate basedOn = null)
            : this(name, basedOn)
        {
            this.Namespace = parent;
        }

        public string GetFullName(string nameSpace)
        {
            return $"{nameSpace}.{this.Name}";
        }
    }
}
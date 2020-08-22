using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class EnumTemplate : AttributeableTempalte, INamespaceChildren
    {
        public List<UsingTemplate> Usings { get; }
        public string Name { get; set; }
        public TypeTemplate BasedOn { get; set; }
        public NamespaceTemplate Namespace { get; }
        public ClassTemplate ParentClass { get; }
        public List<EnumValueTemplate> Values { get; }
        public CommentTemplate Comment { get; set; }

        private EnumTemplate(string name, TypeTemplate basedOn = null)
        {
            this.Name = name;
            this.BasedOn = basedOn;
            this.Usings = new List<UsingTemplate>();
            this.Values = new List<EnumValueTemplate>();
        }

        public EnumTemplate(ClassTemplate parent, string name, TypeTemplate basedOn = null)
            : this(name, basedOn)
        {
            this.ParentClass = parent;
            this.Namespace = parent.Namespace;
        }

        public EnumTemplate(NamespaceTemplate parent, string name, TypeTemplate basedOn = null)
            : this(name, basedOn)
        {
            this.Namespace = parent;
        }
    }
}
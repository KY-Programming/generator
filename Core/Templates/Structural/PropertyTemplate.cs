using System.Diagnostics;
using KY.Generator.Models;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Property {Name}: {Type}")]
    public class PropertyTemplate : AttributeableTempalte
    {
        public string Name { get; set; }
        public TypeTemplate Type { get; }
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsStatic { get; set; }
        public bool IsOptional { get; set; }
        public Visibility Visibility { get; set; }
        public ICodeFragment DefaultValue { get; set; }
        public ICodeFragment Expression { get; set; }
        public ClassTemplate Class { get; }
        public CommentTemplate Comment { get; set; }
        public ICodeFragment Setter { get; set; }
        public ICodeFragment Getter { get; set; }
        public bool Strict { get; set; }

        public PropertyTemplate(ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            this.Class = classTemplate;
            this.Name = name;
            this.Type = type;
            this.HasGetter = true;
            this.HasSetter = true;
            this.Visibility = Visibility.Public;
        }

        public override bool Equals(object obj)
        {
            PropertyTemplate property = obj as PropertyTemplate;
            return property != null && this.Name.Equals(property.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}

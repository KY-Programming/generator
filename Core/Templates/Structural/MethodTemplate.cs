using System.Collections.Generic;
using System.Diagnostics;
using KY.Generator.Models;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Method {Name}")]
    public class MethodTemplate : AttributeableTempalte
    {
        public string Name { get; set; }
        public TypeTemplate Type { get; }
        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public bool IsOverride { get; set; }
        public List<ParameterTemplate> Parameters { get; }
        public MultilineCodeFragment Code { get; }
        public ClassTemplate Class { get; }
        public CommentTemplate Comment { get; set; }
        public List<MethodGenericTemplate> Generics { get; set; }

        public MethodTemplate(ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            this.Class = classTemplate;
            this.Name = name;
            this.Type = type;
            this.Visibility = Visibility.Public;
            this.Parameters = new List<ParameterTemplate>();
            this.Code = new MultilineCodeFragment();
            this.Generics = new List<MethodGenericTemplate>();
        }
    }
}

using System.Collections.Generic;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class AttributeWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public AttributeWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            AttributeTemplate template = (AttributeTemplate)fragment;
            fragments.Add("[")
                     .Add(template.Name);
            if (template.HasValue || template.Properties.Count > 0)
            {
                fragments.Add("(");
                if (template.HasValue)
                {
                    fragments.Add(template.Code, this.Language);
                }
                if (template.Properties.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in template.Properties)
                    {
                        fragments.Add($"{pair.Key} = {this.Language.ConvertValue(pair.Value)}");
                    }
                }
                fragments.Add(")");
            }
            fragments.Add("]");
        }
    }
}
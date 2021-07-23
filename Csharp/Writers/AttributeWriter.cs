using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
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

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AttributeTemplate template = (AttributeTemplate)fragment;
            output.Add("[")
                  .Add(template.Name);
            if (template.HasValue || template.Properties.Count > 0)
            {
                output.Add("(");
                if (template.HasValue)
                {
                    output.Add(template.Code, ", ");
                }
                if (template.Properties.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in template.Properties)
                    {
                        output.Add($"{pair.Key} = {this.Language.ConvertValue(pair.Value)}");
                    }
                }
                output.Add(")");
            }
            output.Add("]");
            if (template.IsInline)
            {
                output.Add(" ");
            }
            else
            {
                output.BreakLine();
            }
        }
    }
}

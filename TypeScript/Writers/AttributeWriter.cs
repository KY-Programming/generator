using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class AttributeWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AttributeTemplate template = (AttributeTemplate)fragment;
            output.Add("@")
                  .Add(template.Name);
            //if (template.HasValue || template.Properties.Count > 0)
            {
                output.Add("(");
                if (template.HasValue)
                {
                    output.Add(template.Code, ", ");
                }
                if (template.Properties.Count > 0)
                {
                    foreach (KeyValuePair<string, ICodeFragment> pair in template.Properties)
                    {
                        output.Add($"{pair.Key} = ").Add(pair.Value);
                    }
                }
                output.Add(")")
                      .BreakLine();
            }
        }
    }
}

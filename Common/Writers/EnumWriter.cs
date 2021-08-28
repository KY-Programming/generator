using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class EnumWriter : Codeable, ITemplateWriter
    {
        private readonly IOptions options;

        public EnumWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            BaseLanguage language = this.options.Language.CastTo<BaseLanguage>();
            EnumTemplate template = (EnumTemplate)fragment;
            output.Add(template.Attributes)
                  .Add(language.ClassScope)
                  .Add(" enum ")
                  .Add(template.Name);
            if (template.BasedOn != null)
            {
                output.Add(" : ").Add(template.BasedOn);
            }
            output.StartBlock();
            EnumValueTemplate last = template.Values.LastOrDefault();
            foreach (EnumValueTemplate enumTemplateValue in template.Values)
            {
                output.Add($"{enumTemplateValue.FormattedName} = ")
                      .Add(enumTemplateValue.Value)
                      .Add(last == enumTemplateValue ? string.Empty : ",")
                      .BreakLine();
            }
            output.EndBlock();
        }
    }
}

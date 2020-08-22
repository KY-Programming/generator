using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class LambdaWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            LambdaTemplate template = (LambdaTemplate)fragment;
            output.Add("(");
            if (template.Parameters != null)
            {
                output.Add(template.Parameters);
            }
            else if (template.ParameterName != null)
            {
                output.Add(template.ParameterName);
            }
            output.Add(")")
                  .Add(" =>");
            if (template.Code is MultilineCodeFragment)
            {
                output.StartBlock();
            }
            else
            {
                output.Add(" ");
            }
            output.Add(template.Code);
            if (template.Code is MultilineCodeFragment)
            {
                output.EndBlock(output.Language.Formatting.StartBlockInNewLine);
            }
        }
    }
}
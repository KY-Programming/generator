using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            TypeScriptTemplate template = (TypeScriptTemplate)fragment;
            output.Add(template.Code);
            if (template.CloseAfter)
            {
                output.CloseLine();
            }
            if (template.BreakAfter)
            {
                output.BreakLine();
            }
            if (template.StartBlockAfter)
            {
                output.StartBlock();
            }
            if (template.EndBlockAfter)
            {
                output.EndBlock();
            }
            if (template.IndentAfter)
            {
                output.Indent();
            }
            if (template.UnindentAfter)
            {
                output.UnIndent();
            }
        }
    }
}
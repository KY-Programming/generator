using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class TypeScriptTemplateExtension
    {
        public static TypeScriptTemplate BreakLine(this TypeScriptTemplate template)
        {
            template.BreakAfter = true;
            return template;
        }

        public static TypeScriptTemplate Close(this TypeScriptTemplate template)
        {
            template.CloseAfter = true;
            return template;
        }

        public static TypeScriptTemplate StartBlock(this TypeScriptTemplate template)
        {
            template.StartBlockAfter = true;
            return template;
        }

        public static TypeScriptTemplate EndBlock(this TypeScriptTemplate template)
        {
            template.EndBlockAfter = true;
            return template;
        }

        public static TypeScriptTemplate Indent(this TypeScriptTemplate template)
        {
            template.IndentAfter = true;
            return template;
        }

        public static TypeScriptTemplate Unindent(this TypeScriptTemplate template)
        {
            template.UnindentAfter = true;
            return template;
        }
    }
}
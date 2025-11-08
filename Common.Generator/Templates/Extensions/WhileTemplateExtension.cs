namespace KY.Generator.Templates.Extensions
{
    public static class WhileTemplateExtension
    {
        public static WhileTemplate WithCode(this WhileTemplate template, ICodeFragment code)
        {
            template.Code.AddLine(code);
            return template;
        }
    }
}

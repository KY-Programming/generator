namespace KY.Generator.Templates.Extensions
{
    public static class DeclareTemplateExtension
    {
        public static LocalVariableTemplate ToLocal(this DeclareTemplate template)
        {
            return template == null ? null : Code.Instance.Local(template.Name);
        }

        public static DeclareTemplate Constant(this DeclareTemplate template)
        {
            template.IsConstant = true;
            return template;
        }
    }
}
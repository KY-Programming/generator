namespace KY.Generator.Templates.Extensions
{
    public static class ExecuteMethodTemplateExtension
    {
        public static ExecuteMethodTemplate WithParameter(this ExecuteMethodTemplate methodTemplate, CodeFragment code)
        {
            methodTemplate.Parameters.Add(code);
            return methodTemplate;
        }
    }
}
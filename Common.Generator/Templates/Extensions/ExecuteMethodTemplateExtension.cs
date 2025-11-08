namespace KY.Generator.Templates.Extensions
{
    public static class ExecuteMethodTemplateExtension
    {
        public static ExecuteMethodTemplate WithParameter(this ExecuteMethodTemplate methodTemplate, ICodeFragment code)
        {
            if (code != null)
            {
                methodTemplate.Parameters.Add(code);
            }
            return methodTemplate;
        }
    }
}
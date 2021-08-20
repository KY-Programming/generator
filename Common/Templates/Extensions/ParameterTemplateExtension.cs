namespace KY.Generator.Templates.Extensions
{
    public static class ParameterTemplateExtension
    {
        public static ParameterTemplate FormatName(this ParameterTemplate parameter, IOptions options, bool force = false)
        {
            parameter.Name = Formatter.FormatParameter(parameter.Name, options, force);
            return parameter;
        }
    }
}

namespace KY.Generator.Templates.Extensions
{
    public static class ChainedCodeFragmentExtension
    {
        public static ExecuteMethodTemplate Method(this ChainedCodeFragment template, string methodName, params CodeFragment[] parameters)
        {
            return new ExecuteMethodTemplate(methodName, parameters).Chain(template);
        }

        public static ExecuteFieldTemplate Field(this ChainedCodeFragment template, string fieldName)
        {
            return new ExecuteFieldTemplate(fieldName).Chain(template);
        }

        public static ExecutePropertyTemplate Property(this ChainedCodeFragment template, string propertyName)
        {
            return new ExecutePropertyTemplate(propertyName).Chain(template);
        }

        public static ExecuteGenericMethodTemplate GenericMethod(this ChainedCodeFragment template, string methodName, params TypeTemplate[] types)
        {
            return new ExecuteGenericMethodTemplate(methodName, types).Chain(template);
        }

        public static AccessIndexTemplate Index(this ChainedCodeFragment template, CodeFragment code)
        {
            return new AccessIndexTemplate(code).Chain(template);
        }

        public static AssignTemplate Assign(this ChainedCodeFragment template, CodeFragment code)
        {
            return new AssignTemplate(code).Chain(template);
        }

        public static OperatorTemplate And(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.And).Chain(template);
        }

        public static OperatorTemplate Or(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.Or).Chain(template);
        }

        public static OperatorTemplate Equals(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.Equals).Chain(template);
        }

        public static OperatorTemplate NotEquals(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.NotEquals).Chain(template);
        }

        public static OperatorTemplate Lower(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.Lower).Chain(template);
        }

        public static OperatorTemplate LowerThan(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.LowerThan).Chain(template);
        }

        public static OperatorTemplate Greater(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.Greater).Chain(template);
        }

        public static OperatorTemplate GreaterThan(this ChainedCodeFragment template)
        {
            return new OperatorTemplate(Operator.GreaterThan).Chain(template);
        }

        public static NullValueTemplate Null(this ChainedCodeFragment template)
        {
            return new NullValueTemplate().Chain(template);
        }

        public static LocalVariableTemplate Local(this ChainedCodeFragment template, string name)
        {
            return new LocalVariableTemplate(name).Chain(template);
        }

        public static ThisTemplate This(this ChainedCodeFragment template)
        {
            return new ThisTemplate().Chain(template);
        }

        public static T NewLine<T>(this T template)
            where T : ChainedCodeFragment
        {
            template.NewLineAfter = true;
            return template;
        }

        public static AsTemplate As(this ChainedCodeFragment template, TypeTemplate type)
        {
            return new AsTemplate(type).Chain(template);
        }

        public static NotTemplate Not(this ChainedCodeFragment template)
        {
            return new NotTemplate().Chain(template);
        }

        private static T Chain<T, TPrevious>(this T template, TPrevious previous)
            where T : ChainedCodeFragment
            where TPrevious : ChainedCodeFragment
        {
            previous.Next = template;
            template.Previous = previous;
            return template;
        }
    }
}
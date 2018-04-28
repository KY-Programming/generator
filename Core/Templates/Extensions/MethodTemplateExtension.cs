using System.Collections.Generic;
using KY.Generator.Models;

namespace KY.Generator.Templates.Extensions
{
    public static class MethodTemplateExtension
    {
        public static MethodTemplate Internal(this MethodTemplate methodTemplate)
        {
            methodTemplate.Visibility = Visibility.Internal;
            return methodTemplate;
        }

        public static MethodTemplate Protected(this MethodTemplate methodTemplate)
        {
            methodTemplate.Visibility = Visibility.Protected;
            return methodTemplate;
        }

        public static MethodTemplate Private(this MethodTemplate methodTemplate)
        {
            methodTemplate.Visibility = Visibility.Private;
            return methodTemplate;
        }

        public static MethodTemplate Static(this MethodTemplate methodTemplate)
        {
            methodTemplate.IsStatic = true;
            return methodTemplate;
        }

        public static MethodTemplate Override(this MethodTemplate methodTemplate)
        {
            methodTemplate.IsOverride = true;
            return methodTemplate;
        }

        public static ParameterTemplate AddParameter(this MethodTemplate methodTemplate, TypeTemplate type, string name, CodeFragment defaultValue = null)
        {
            var parameter = new ParameterTemplate(type, name, defaultValue);
            methodTemplate.Parameters.Add(parameter);
            return parameter;
        }

        public static MethodTemplate WithParameter(this MethodTemplate methodTemplate, ParameterTemplate parameter)
        {
            methodTemplate.Parameters.Add(parameter);
            return methodTemplate;
        }

        public static MethodTemplate WithParameter(this MethodTemplate methodTemplate, TypeTemplate type, string name, CodeFragment defaultValue = null)
        {
            methodTemplate.AddParameter(type, name, defaultValue);
            return methodTemplate;
        }

        public static MethodTemplate WithParameters(this MethodTemplate methodTemplate, IEnumerable<ParameterTemplate> parameters)
        {
            methodTemplate.Parameters.AddRange(parameters);
            return methodTemplate;
        }

        public static MethodTemplate WithComment(this MethodTemplate methodTemplate, string description)
        {
            methodTemplate.Comment = new CommentTemplate(description, CommentType.Summary);
            return methodTemplate;
        }

        public static MethodTemplate WithCode(this MethodTemplate methodTemplate, CodeFragment code)
        {
            methodTemplate.Code.Fragments.Add(code);
            return methodTemplate;
        }
    }
}
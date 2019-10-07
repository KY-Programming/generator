using KY.Generator.Csharp.Templates;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Extensions
{
    public static class ConstructorTemplateExtension
    {
        public static ConstructorTemplate WithBaseConstructor(this ConstructorTemplate constructorTemplate, params ICodeFragment[] parameters)
        {
            constructorTemplate.ConstructorCall = Code.Instance.Method("base", parameters);
            return constructorTemplate;
        }

        public static ConstructorTemplate WithThisConstructor(this ConstructorTemplate constructorTemplate, params ICodeFragment[] parameters)
        {
            constructorTemplate.ConstructorCall = Code.Instance.Method("this", parameters);
            return constructorTemplate;
        }

        public static ConstructorTemplate WithParameter(this ConstructorTemplate constructorTemplate, TypeTemplate type, string name, ICodeFragment defaultValue = null)
        {
            constructorTemplate.Parameters.Add(new ParameterTemplate(type, name, defaultValue));
            return constructorTemplate;
        }
    }
}
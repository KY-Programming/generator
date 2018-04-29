using KY.Generator.Templates;

namespace KY.Generator.Extensions
{
    public static class ConstructorTemplateExtension
    {
        public static ConstructorTemplate WithBaseConstructor(this ConstructorTemplate constructorTemplate, params CodeFragment[] parameters)
        {
            constructorTemplate.BaseConstructor = Code.Method("base", parameters);
            return constructorTemplate;
        }

        public static ConstructorTemplate WithThisConstructor(this ConstructorTemplate constructorTemplate, params CodeFragment[] parameters)
        {
            constructorTemplate.BaseConstructor = Code.Method("this", parameters);
            return constructorTemplate;
        }

        public static ConstructorTemplate WithParameter(this ConstructorTemplate constructorTemplate, TypeTemplate type, string name, CodeFragment defaultValue = null)
        {
            constructorTemplate.Parameters.Add(new ParameterTemplate(type, name, defaultValue));
            return constructorTemplate;
        }
    }
}
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class ConstructorTemplateExtension
    {
        public static ConstructorTemplate WithSuper(this ConstructorTemplate constructorTemplate, params ICodeFragment[] parameters)
        {
            if (constructorTemplate.Class.BasedOn != null && !constructorTemplate.Class.BasedOn.ToType().IsInterface)
            {
                constructorTemplate.Code.Fragments.Insert(0, KY.Generator.Code.Method("super", parameters));
            }
            return constructorTemplate;
        }

        public static ConstructorTemplate WithParameter(this ConstructorTemplate constructorTemplate, TypeTemplate type, string name, ICodeFragment defaultValue = null)
        {
            constructorTemplate.Parameters.Add(new ParameterTemplate(type, name, defaultValue));
            return constructorTemplate;
        }
    }
}
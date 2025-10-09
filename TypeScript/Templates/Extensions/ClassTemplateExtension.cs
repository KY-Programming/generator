using KY.Core;
using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates.Extensions;

public static class ClassTemplateExtension
{
    public static MethodTemplate AddOverload(this MethodTemplate method, Action<TypeScriptMethodOverloadTemplate> action)
    {
        TypeScriptMethodTemplate typeScriptMethod = method as TypeScriptMethodTemplate ?? new TypeScriptMethodTemplate(method);
        TypeScriptMethodOverloadTemplate overload = new();
        action(overload);
        typeScriptMethod.Overloads.Add(overload);
        method.Class.Methods.Replace(method, typeScriptMethod);
        return typeScriptMethod;
    }
}

public static class TypeScriptMethodOverloadTemplateExtension
{
    public static TypeScriptMethodOverloadTemplate WithGeneric(this TypeScriptMethodOverloadTemplate methodTemplate, string alias, TypeTemplate? defaultType = null)
    {
        methodTemplate.Generics.Add(new MethodGenericTemplate(alias, defaultType));
        return methodTemplate;
    }

    public static ParameterTemplate AddParameter(this TypeScriptMethodOverloadTemplate methodTemplate, TypeTemplate type, string name, ICodeFragment? defaultValue = null)
    {
        ParameterTemplate parameter = new(type, name, defaultValue);
        methodTemplate.Parameters.Add(parameter);
        return parameter;
    }

    public static TypeScriptMethodOverloadTemplate WithParameter(this TypeScriptMethodOverloadTemplate methodTemplate, ParameterTemplate parameter)
    {
        methodTemplate.Parameters.Add(parameter);
        return methodTemplate;
    }

    public static TypeScriptMethodOverloadTemplate WithParameter(this TypeScriptMethodOverloadTemplate methodTemplate, TypeTemplate type, string name, ICodeFragment? defaultValue = null)
    {
        methodTemplate.AddParameter(type, name, defaultValue);
        return methodTemplate;
    }

    public static TypeScriptMethodOverloadTemplate WithParameters(this TypeScriptMethodOverloadTemplate methodTemplate, IEnumerable<ParameterTemplate> parameters)
    {
        methodTemplate.Parameters.AddRange(parameters);
        return methodTemplate;
    }

    public static TypeScriptMethodOverloadTemplate WithComment(this TypeScriptMethodOverloadTemplate methodTemplate, string description)
    {
        methodTemplate.Comment = new CommentTemplate(description, CommentType.Summary);
        return methodTemplate;
    }

    public static TypeScriptMethodOverloadTemplate WithReturnType(this TypeScriptMethodOverloadTemplate methodTemplate, TypeTemplate type)
    {
        methodTemplate.ReturnType = type;
        return methodTemplate;
    }
}

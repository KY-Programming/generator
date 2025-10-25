using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript;

public static class TypeScriptCode
{
    public static TypeScriptTemplate TypeScript(this Code _, string code)
    {
        return new TypeScriptTemplate(code);
    }

    public static NullTemplate Undefined(this Code _)
    {
        return new NullTemplate();
    }

    public static NullValueTemplate Undefined(this ChainedCodeFragment template)
    {
        return new NullValueTemplate().Chain(template);
    }

    public static ForceNullTemplate ForceNull(this Code _)
    {
        return new ForceNullTemplate();
    }

    public static ForceNullValueTemplate ForceNull(this ChainedCodeFragment template)
    {
        return new ForceNullValueTemplate().Chain(template);
    }

    public static TypeScriptUnionTypeTemplate UnionType(this Code _, params TypeTemplate[] types)
    {
        return new TypeScriptUnionTypeTemplate(types);
    }
}

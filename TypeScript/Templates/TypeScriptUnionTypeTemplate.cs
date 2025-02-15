using KY.Core;
using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates;

public class TypeScriptUnionTypeTemplate : TypeTemplate
{
    public List<TypeTemplate> Types { get; } = [];

    public TypeScriptUnionTypeTemplate(IEnumerable<TypeTemplate>? types = null)
    {
        if (types == null)
        {
            return;
        }
        foreach (TypeTemplate type in types)
        {
            if (type is TypeScriptUnionTypeTemplate unionType)
            {
                foreach (TypeTemplate subType in unionType.Types)
                {
                    this.Types.AddIfNotExists(subType);
                }
            }
            else
            {
                this.Types.Add(type);
            }
        }
    }
}

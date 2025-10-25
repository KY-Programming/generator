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
            if (type == null)
            {
                continue;
            }
            if (type is TypeScriptUnionTypeTemplate unionType)
            {
                foreach (TypeTemplate subType in unionType.Types)
                {
                    this.AddType(subType);
                }
            }
            else
            {
                this.AddType(type);
            }
        }
    }

    private void AddType(TypeTemplate type)
    {
        if (this.Types.Any(x => x.Equals(type)))
        {
            return;
        }
        this.Types.Add(type);
    }
}

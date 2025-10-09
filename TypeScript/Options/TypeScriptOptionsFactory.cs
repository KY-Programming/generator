using System.Reflection;

namespace KY.Generator.TypeScript;

public class TypeScriptOptionsFactory : IOptionsFactory
{
    public bool CanCreate(Type optionsType)
    {
        return optionsType == typeof(TypeScriptOptions);
    }

    public object Create(Type optionsType, object key, object? parent, object global)
    {
        return new TypeScriptOptions(parent as TypeScriptOptions, global as TypeScriptOptions, key);
    }

    public object CreateGlobal(Type optionsType, object key, object? parent)
    {
        return key switch
        {
            Assembly assembly => this.CreateFromCustomAttributes(assembly.GetCustomAttributes(), key, parent as TypeScriptOptions),
            MemberInfo member => this.CreateFromCustomAttributes(member.GetCustomAttributes(), key, parent as TypeScriptOptions),
            ParameterInfo parameter => this.CreateFromCustomAttributes(parameter.GetCustomAttributes(), key, parent as TypeScriptOptions),
            Options.GlobalKey => new TypeScriptOptions(parent as TypeScriptOptions, null, "global"),
            _ => new TypeScriptOptions(parent as TypeScriptOptions, null, key)
            // _ => throw new InvalidOperationException($"Could not create {nameof(TypeScriptOptions)} {key.GetType()}")
        };
    }

    private TypeScriptOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, object key, TypeScriptOptions? parent)
    {
        TypeScriptOptions options = new(parent, null, key);
        foreach (Attribute attribute in customAttributes)
        {
            switch (attribute)
            {
                case GenerateStrictAttribute strictAttribute:
                    options.Strict = strictAttribute.Strict;
                    break;
                case GenerateNoIndexAttribute:
                    options.NoIndex = true;
                    break;
                case GenerateForceIndexAttribute:
                    options.ForceIndex = true;
                    break;
                case GenerateModelOutputAttribute modelOutputAttribute:
                    options.ModelOutput = modelOutputAttribute.RelativePath;
                    break;
            }
        }
        return options;
    }
}

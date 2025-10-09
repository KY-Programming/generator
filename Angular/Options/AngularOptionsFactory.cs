using System.Reflection;

namespace KY.Generator.Angular;

public class AngularOptionsFactory : IOptionsFactory
{
    public bool CanCreate(Type optionsType)
    {
        return optionsType == typeof(AngularOptions);
    }

    public object Create(Type optionsType, object key, object? parent, object global)
    {
        return new AngularOptions(parent as AngularOptions, global as AngularOptions, key);
    }

    public object CreateGlobal(Type optionsType, object key, object? parent)
    {
        return key switch
        {
            Assembly assembly => this.CreateFromCustomAttributes(assembly.GetCustomAttributes(), key, parent as AngularOptions),
            MemberInfo member => this.CreateFromCustomAttributes(member.GetCustomAttributes(), key, parent as AngularOptions),
            ParameterInfo parameter => this.CreateFromCustomAttributes(parameter.GetCustomAttributes(), key, parent as AngularOptions),
            Options.GlobalKey => new AngularOptions(parent as AngularOptions, null, "global"),
            _ => new AngularOptions(parent as AngularOptions, null, key)
            // _ => throw new InvalidOperationException($"Could not create {nameof(AngularOptions)} {key.GetType()}")
        };
    }

    private AngularOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, object key, AngularOptions parent)
    {
        AngularOptions options = new(parent, null, key);
        foreach (Attribute attribute in customAttributes)
        {
            switch (attribute)
            {
                case GenerateServiceOutputAttribute serviceOutputAttribute:
                    options.ServiceOutput = serviceOutputAttribute.RelativePath;
                    break;
                case GenerateModelOutputAttribute modelOutputAttribute:
                    options.ModelOutput = modelOutputAttribute.RelativePath;
                    break;
            }
        }
        return options;
    }
}

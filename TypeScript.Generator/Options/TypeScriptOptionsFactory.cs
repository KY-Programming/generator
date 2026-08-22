using System.Reflection;

namespace KY.Generator.TypeScript;

public class TypeScriptOptionsFactory : IOptionsFactory, IConfigurableOptionsFactory
{
    public string SettingsSection => "typescript";
    public string SettingsSectionDescription => "Options that only apply to generated TypeScript";
    public Type OptionsType => typeof(TypeScriptOptions);

    public IReadOnlyList<SettingsOption> SettingsOptions { get; } =
    [
        SettingsOption.For<TypeScriptOptions, bool>("forceIndex", false, "Write an index.ts even for a folder that opted out of it",
                                                  (options, value) => options.ForceIndex = value, options => options.ForceIndex),
        SettingsOption.For<TypeScriptOptions, bool>("noIndex", false, "Do not write an index.ts next to the generated files",
                                                  (options, value) => options.NoIndex = value, options => options.NoIndex),
        SettingsOption.For<TypeScriptOptions, bool>("strict", true, "Generate code that is valid for the strict mode of TypeScript",
                                                  (options, value) => options.Strict = value, options => options.Strict)
    ];

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
            Options.RootKey => new TypeScriptOptions(parent as TypeScriptOptions, null, "global"),
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
                case GenerateNonStrictAttribute nonStrictAttribute:
                    options.Strict = !nonStrictAttribute.NonStrict;
                    break;
                case GenerateNoIndexAttribute:
                    options.NoIndex = true;
                    break;
                case GenerateForceIndexAttribute:
                    options.ForceIndex = true;
                    break;
            }
        }
        return options;
    }
}

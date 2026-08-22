using System.Reflection;

namespace KY.Generator.Angular;

public class AngularOptionsFactory : IOptionsFactory, IConfigurableOptionsFactory
{
    public string SettingsSection => "angular";
    public string SettingsSectionDescription => "Options that only apply to generated Angular code";
    public Type OptionsType => typeof(AngularOptions);

    public IReadOnlyList<SettingsOption> SettingsOptions { get; } =
    [
        SettingsOption.For<AngularOptions, string>("serviceOutput", AngularOptions.DefaultServiceOutput, "Relative path the generated services are written to",
                                                 (options, value) => options.ServiceOutput = value, options => options.ServiceOutput),
        SettingsOption.For<AngularOptions, bool>("withSignals", false, "Generate signals instead of plain properties",
                                               (options, value) => options.WithSignals = value, options => options.WithSignals)
    ];

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
            Options.RootKey => new AngularOptions(parent as AngularOptions, null, "global"),
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
                case GenerateWithSignalsAttribute:
                    options.WithSignals = true;
                    break;
            }
        }
        return options;
    }
}

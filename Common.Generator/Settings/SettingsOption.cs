using System.Reflection;
using KY.Generator.Extensions;
using Newtonsoft.Json.Linq;

namespace KY.Generator;

/// <summary>
/// One option a <c>ky-generator.json</c> may set, declared by the <see cref="IConfigurableOptionsFactory"/> that owns
/// the section it lives in. The declaration is the single source of truth for the option: it applies the value, it
/// tells <c>settings-init</c> what to write and <c>settings-show</c> what to list, and a key that has no declaration is
/// reported as a typo instead of being silently ignored
/// </summary>
public class SettingsOption
{
    private readonly Action<object, JToken>? apply;
    private readonly Func<object, object?>? read;
    private readonly Func<object, object?>? selectChildTarget;

    public string Name { get; }
    public Type ValueType { get; }
    public object? DefaultValue { get; }

    /// <summary>
    /// A value that shows what the option expects, for an option that has no default. Only used to write a commented
    /// out line by <c>settings-init</c> - null there would show the shape of nothing
    /// </summary>
    public object? Example { get; }

    /// <summary>
    /// A key that is valid but not written by <c>settings-init</c>, because it is not something to suggest to
    /// everybody who initializes a settings file
    /// </summary>
    public bool Hidden { get; }
    public string Description { get; }

    /// <summary>
    /// The options of a nested object, e.g. the <c>formatting</c> of the <c>options</c> section. Empty for a value
    /// </summary>
    public IReadOnlyList<SettingsOption> Children { get; }

    private SettingsOption(string name, Type valueType, object? defaultValue, object? example, bool hidden, string description,
                           Action<object, JToken>? apply, Func<object, object?>? read,
                           Func<object, object?>? selectChildTarget, IReadOnlyList<SettingsOption>? children)
    {
        this.Name = name;
        this.ValueType = valueType;
        this.DefaultValue = defaultValue;
        this.Example = example;
        this.Hidden = hidden;
        this.Description = description;
        this.apply = apply;
        this.read = read;
        this.selectChildTarget = selectChildTarget;
        this.Children = children ?? [];
    }

    /// <summary>
    /// Declares a value that is written to <typeparamref name="TOptions"/>
    /// </summary>
    public static SettingsOption For<TOptions, TValue>(string name, TValue? defaultValue, string description,
                                                       Action<TOptions, TValue> setter, Func<TOptions, object?> getter,
                                                       object? example = null, bool hidden = false)
    {
        return new SettingsOption(name, typeof(TValue), defaultValue, example, hidden, description,
                                  (options, token) => setter((TOptions)options, token.ToObject<TValue>()!),
                                  options => getter((TOptions)options),
                                  null, null);
    }

    /// <summary>
    /// Declares a nested object whose own options are written to the object <paramref name="selectTarget"/> returns
    /// </summary>
    public static SettingsOption Group<TOptions>(string name, string description, Func<TOptions, object?> selectTarget,
                                               IReadOnlyList<SettingsOption> children)
    {
        return new SettingsOption(name, typeof(JObject), null, null, false, description, null, null,
                                  options => selectTarget((TOptions)options), children);
    }

    /// <summary>
    /// Declares one option per public read/write property of <typeparamref name="T"/> that carries a simple value.
    /// Used for option objects that are plain property bags - hand-written declarations for those would only be a
    /// second list to keep in sync
    /// </summary>
    public static IReadOnlyList<SettingsOption> FromProperties<T>(T defaults, IReadOnlyDictionary<string, object>? examples = null)
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(property => property.CanRead && property.CanWrite && IsSimple(property.PropertyType))
                        .OrderBy(property => property.Name)
                        .Select(property => new SettingsOption(
                                    property.Name.ToCamelCase(), property.PropertyType, ReadSafe(property, defaults),
                                    GetExample(examples, property.Name.ToCamelCase()), false, string.Empty,
                                    (options, token) => property.SetValue(options, token.ToObject(property.PropertyType)),
                                    ReadSafe(property),
                                    null, null))
                        .ToList();
    }

    /// <summary>
    /// Writes the value of one settings key to the options object it belongs to
    /// </summary>
    public void Apply(object options, JToken value)
    {
        if (this.apply != null)
        {
            this.apply(options, value);
            return;
        }
        object? childTarget = this.selectChildTarget?.Invoke(options);
        if (childTarget == null || value is not JObject childValues)
        {
            return;
        }
        foreach (JProperty property in childValues.Properties())
        {
            this.Children.FirstOrDefault(child => child.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                ?.Apply(childTarget, property.Value);
        }
    }

    /// <summary>
    /// The value the options object currently carries, for <c>settings-show</c>
    /// </summary>
    public object? Read(object options)
    {
        return this.read == null ? null : this.read(options);
    }

    /// <summary>
    /// The object the children of this group are read from and written to
    /// </summary>
    public object? SelectChildTarget(object options)
    {
        return this.selectChildTarget?.Invoke(options);
    }

    private static object? GetExample(IReadOnlyDictionary<string, object>? examples, string name)
    {
        return examples != null && examples.TryGetValue(name, out object example) ? example : null;
    }

    private static bool IsSimple(Type type)
    {
        Type underlying = Nullable.GetUnderlyingType(type) ?? type;
        return underlying.IsEnum || underlying == typeof(string) || underlying == typeof(bool)
               || underlying == typeof(int) || underlying == typeof(long) || underlying == typeof(double);
    }

    /// <summary>
    /// A getter that resolves through a chain of other option objects can throw while nothing is set up yet. The
    /// value is only shown to the user, so an unreadable one is not worth failing over
    /// </summary>
    private static Func<object, object?> ReadSafe(PropertyInfo property)
    {
        return target =>
        {
            try
            {
                return property.GetValue(target);
            }
            catch
            {
                return null;
            }
        };
    }

    private static object? ReadSafe(PropertyInfo property, object? target)
    {
        return target == null ? null : ReadSafe(property)(target);
    }
}

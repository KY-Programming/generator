namespace KY.Generator;

/// <summary>
/// Notifies the generator to load the specified assembly with its dependencies and search for plugins.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class GeneratePluginAttribute : Attribute
{ }

namespace KY.Generator.Command.Extensions;

public static class OptionsExtension
{
    public static void SetFromParameter(this GeneratorOptions options, GeneratorCommandParameters parameters)
    {
        if (parameters.NoHeader != null)
        {
            options.AddHeader = !parameters.NoHeader.Value;
        }
        if (parameters.FormatNames != null)
        {
            options.FormatNames = parameters.FormatNames.Value;
        }
        if (parameters.FieldsToProperties != null)
        {
            options.FieldsToProperties = parameters.FieldsToProperties.Value;
        }
        if (parameters.PropertiesToFields != null)
        {
            options.PropertiesToFields = parameters.PropertiesToFields.Value;
        }
        if (parameters.SkipNamespace != null)
        {
            options.SkipNamespace = parameters.SkipNamespace.Value;
        }
        if (parameters.PreferInterfaces != null)
        {
            options.PreferInterfaces = parameters.PreferInterfaces.Value;
        }
        if (parameters.WithOptionalProperties != null)
        {
            options.WithOptionalProperties = parameters.WithOptionalProperties.Value;
        }
    }
}
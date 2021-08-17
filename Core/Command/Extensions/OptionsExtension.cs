using System.Collections.Generic;
using System.Linq;
using KY.Generator.Transfer;

namespace KY.Generator.Command.Extensions
{
    public static class OptionsExtension
    {
        public static void SetFromParameter(this IOptions options, GeneratorCommandParameters parameters)
        {
            options.AddHeader = !parameters.SkipHeader;
            options.FormatNames = parameters.FormatNames;
            options.FieldsToProperties = parameters.FieldsToProperties;
            options.PropertiesToFields = parameters.PropertiesToFields;
            options.SkipNamespace = parameters.SkipNamespace;
            options.PreferInterfaces = parameters.PreferInterfaces;
            options.WithOptionalProperties = parameters.WithOptionalProperties;
        }

        public static void SetOutputId(this IOptions options, IEnumerable<ITransferObject> transferObjects)
        {
            options.OutputId = transferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
        }
    }
}

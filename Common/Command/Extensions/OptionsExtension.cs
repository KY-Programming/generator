﻿using System.Collections.Generic;
using System.Linq;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator.Command.Extensions
{
    public static class OptionsExtension
    {
        public static void SetFromParameter(this IOptions options, GeneratorCommandParameters parameters)
        {
            if (parameters.NoHeader.HasValue)
            {
                options.AddHeader = parameters.NoHeader.Value;
            }
            options.FormatNames = parameters.FormatNames;
            options.FieldsToProperties = parameters.FieldsToProperties;
            options.PropertiesToFields = parameters.PropertiesToFields;
            options.SkipNamespace = parameters.SkipNamespace;
            options.PreferInterfaces = parameters.PreferInterfaces;
            options.WithOptionalProperties = parameters.WithOptionalProperties;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Command
{
    public static class ParameterListExtension
    {
        public static string GetValue(this IList<CommandParameter> list, string parameter)
        {
            return list.OfType<CommandValueParameter>().FirstOrDefault(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static bool GetBoolValue(this IList<CommandParameter> list, string parameter, bool defaultValue = true)
        {
            string value = list.GetValue(parameter);
            return bool.TrueString.Equals(value, StringComparison.InvariantCultureIgnoreCase) || !bool.FalseString.Equals(value, StringComparison.InvariantCultureIgnoreCase) && defaultValue;
        }
    }
}
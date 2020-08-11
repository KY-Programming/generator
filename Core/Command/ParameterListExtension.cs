using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Command
{
    public static class ParameterListExtension
    {
        public static bool Exists(this IList<ICommandParameter> list, string parameter)
        {
            return list.Any(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string GetString(this IList<ICommandParameter> list, string parameter)
        {
            return list.OfType<CommandValueParameter>().FirstOrDefault(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static bool GetBool(this IList<ICommandParameter> list, string parameter, bool defaultValue = false)
        {
            if (list.OfType<CommandParameter>().Any(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            string value = list.GetString(parameter);
            if (bool.TrueString.Equals(value, StringComparison.InvariantCultureIgnoreCase) || string.Empty.Equals(value))
            {
                return true;
            }
            if (bool.FalseString.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return defaultValue;
        }

        public static int GetInt(this IList<ICommandParameter> list, string parameter, int defaultValue = default)
        {
            string value = list.GetString(parameter);
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public static TimeSpan GetTimeSpan(this IList<ICommandParameter> list, string parameter, TimeSpan defaultValue = default)
        {
            string value = list.GetString(parameter);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if ((value.Contains(".") || value.Contains(":")) && TimeSpan.TryParse(value, out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            return int.TryParse(value, out int result) ? TimeSpan.FromMilliseconds(result) : defaultValue;
        }

        public static T GetEnum<T>(this IList<ICommandParameter> list, string parameter, T defaultValue = default)
        {
            string value = list.GetString(parameter);
            return EnumHelper.Parse(value, defaultValue);
        }
    }
}
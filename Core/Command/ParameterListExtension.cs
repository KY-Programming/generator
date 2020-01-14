using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Command
{
    public static class ParameterListExtension
    {
        public static bool Exists(this IEnumerable<CommandParameter> list, string parameter)
        {
            return list.Any(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string GetString(this IEnumerable<CommandParameter> list, string parameter)
        {
            return list.FirstOrDefault(x => parameter.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static bool GetBool(this IEnumerable<CommandParameter> list, string parameter, bool defaultValue = false)
        {
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

        public static int GetInt(this IEnumerable<CommandParameter> list, string parameter, int defaultValue = default)
        {
            string value = list.GetString(parameter);
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public static TimeSpan GetTimeSpan(this IEnumerable<CommandParameter> list, string parameter, TimeSpan defaultValue = default)
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
    }
}
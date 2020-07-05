using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Command
{
    public static class CommandParameterParser
    {
        public static IEnumerable<ICommandParameter> Parse(params string[] parameters)
        {
            return parameters.Select(parameter => parameter.StartsWith("-") ? (ICommandParameter)CommandValueParameter.Parse(parameter) : new CommandParameter(parameter));
        }
    }
}   
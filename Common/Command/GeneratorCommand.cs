using System.Collections;
using System.Reflection;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Models;

namespace KY.Generator.Command;

public abstract class GeneratorCommand<T> : IGeneratorCommand
    where T : GeneratorCommandParameters, new()
{
    public T Parameters { get; } = new();
    GeneratorCommandParameters IGeneratorCommand.Parameters => this.Parameters;
    public List<CliCommandParameter> OriginalParameters { get; set; }

    public bool Parse()
    {
        Type type = typeof(T);
        Dictionary<string, PropertyInfo> mapping = new();
        foreach (PropertyInfo property in type.GetProperties())
        {
            foreach (GeneratorParameterAttribute attribute in property.GetCustomAttributes<GeneratorParameterAttribute>())
            {
                mapping.AddIfNotExists(CliCommandParameter.FormatName(attribute.ParameterName), property);
            }
            foreach (GeneratorGlobalParameterAttribute attribute in property.GetCustomAttributes<GeneratorGlobalParameterAttribute>().Where(x => !string.IsNullOrEmpty(x.ParameterName)))
            {
                mapping.AddIfNotExists(CliCommandParameter.FormatName(attribute.ParameterName), property);
            }
            mapping.AddIfNotExists(CliCommandParameter.FormatName(property.Name), property);
        }
        foreach (CliCommandParameter parameter in this.OriginalParameters)
        {
            string parameterName = parameter.Name.ToLowerInvariant();
            if (!mapping.ContainsKey(parameterName))
            {
                Logger.Warning($"Unknown parameter '{parameter.Name}' on '{this.GetType().Name}' command");
                continue;
            }
            PropertyInfo property = mapping[parameterName];
            bool isList = property.PropertyType.Name.StartsWith("List`");
            if (isList && this.OriginalParameters.Count(p => p.Name == parameter.Name) > 1)
            {
                IList list = property.GetMethod.Invoke(this.Parameters, null) as IList;
                if (list == null)
                {
                    Logger.Error($"Can not write parameter '{parameter.Name}' on '{this.GetType().Name}' command. Parameter is from type List and has to be always initialized");
                    return false;
                }
                Type itemType = property.PropertyType.GetGenericArguments().First();
                if (!GeneratorCommand.ParameterParser.ContainsKey(itemType))
                {
                    Logger.Error($"Can not write parameter '{parameter.Name}' on '{this.GetType().Name}' command. No converter from List<string> to List<{itemType.FullName}> found. Use {nameof(GeneratorCommand)}.{nameof(GeneratorCommand.ParameterParser)} to set a custom parser. Do not map List<T> map T instead");
                    return false;
                }
                list.Add(GeneratorCommand.ParameterParser[itemType].Invoke(parameter.Value));
            }
            else
            {
                if (!property.CanWrite)
                {
                    Logger.Warning($"Can not write parameter '{parameter.Name}' on '{this.GetType().Name}' command. Ensure command property has a public setter.");
                    continue;
                }

                if (!GeneratorCommand.ParameterParser.ContainsKey(property.PropertyType))
                {
                    Logger.Error($"Can not write parameter '{parameter.Name}' on '{this.GetType().Name}' command. No converter from System.String to {property.PropertyType.FullName} found. Use {nameof(GeneratorCommand)}.{nameof(GeneratorCommand.ParameterParser)} to set a custom parser");
                    return false;
                }
                try
                {
                    property.SetMethod.Invoke(this.Parameters, [GeneratorCommand.ParameterParser[property.PropertyType].Invoke(parameter.Value)]);
                }
                catch (Exception exception)
                {
                    Logger.Error($"Can not set parameter '{this.GetType().Name}.{parameter.Name}'. {exception.Message}");
                    return false;
                }
            }
        }
        return true;
    }

    public virtual void Prepare()
    { }

    public virtual void FollowUp()
    { }

    public abstract IGeneratorCommandResult Run();

    protected SuccessResult Success()
    {
        return new SuccessResult();
    }

    protected ErrorResult Error()
    {
        return new ErrorResult();
    }

    protected SwitchContextResult SwitchContext(SwitchableFramework switchToFramework)
    {
        return new SwitchContextResult(null, switchToFramework);
    }

    protected SwitchContextResult SwitchContext(ProcessorArchitecture? switchToArchitecture, SwitchableFramework switchToFramework = SwitchableFramework.None)
    {
        return new SwitchContextResult(switchToArchitecture, switchToFramework);
    }

    protected SwitchAsyncResult SwitchAsync()
    {
        return new SwitchAsyncResult();
    }

    public override string ToString()
    {
        return $" {ToCommand(this.GetType().Name)} {(this.OriginalParameters == null ? string.Join(" ", this.ParametersToString()) : string.Join(" ", this.OriginalParameters))}";
    }

    protected static IEnumerable<string> ToCommand(string className)
    {
        string baseName = className.TrimEnd("Command").ToKebabCase();
        yield return baseName;
        yield return baseName.Replace("'", string.Empty);
    }

    private IEnumerable<string> ParametersToString()
    {
        foreach (PropertyInfo property in this.Parameters.GetType().GetProperties().Where(x => x.CanRead))
        {
            object value = property.GetMethod.Invoke(this.Parameters, null);
            if (value != null)
            {
                yield return new CliCommandParameter(property.Name, value.ToString()).ToString();
            }
        }
    }
}

public static class GeneratorCommand
{
    internal static Dictionary<Type, Func<string, object>> ParameterParser { get; } = new();

    static GeneratorCommand()
    {
        AddParser(value => value);
        AddParser(value => !value?.Equals(bool.FalseString, StringComparison.CurrentCultureIgnoreCase) ?? true);
        AddParser(value => !value?.Equals(bool.FalseString, StringComparison.CurrentCultureIgnoreCase));
        AddParser(value => value?.Split(',').Select(x => !x.Trim().Equals(bool.FalseString, StringComparison.CurrentCultureIgnoreCase)).ToList());
        AddParser(byte.Parse);
        AddParser(value => value == null ? (byte?)null : byte.Parse(value));
        AddParser(value => value?.Split(',').Select(byte.Parse).ToList());
        AddParser(sbyte.Parse);
        AddParser(value => value == null ? (sbyte?)null : sbyte.Parse(value));
        AddParser(value => value?.Split(',').Select(sbyte.Parse).ToList());
        AddParser(char.Parse);
        AddParser(value => value == null ? (char?)null : char.Parse(value));
        AddParser(value => value?.Split(',').Select(char.Parse).ToList());
        AddParser(DateTime.Parse);
        AddParser(value => value == null ? (DateTime?)null : DateTime.Parse(value));
        AddParser(value => value?.Split(',').Select(DateTime.Parse).ToList());
        AddParser(decimal.Parse);
        AddParser(value => value == null ? (decimal?)null : decimal.Parse(value));
        AddParser(value => value?.Split(',').Select(decimal.Parse).ToList());
        AddParser(short.Parse);
        AddParser(value => value == null ? (short?)null : short.Parse(value));
        AddParser(value => value?.Split(',').Select(short.Parse).ToList());
        AddParser(int.Parse);
        AddParser(value => value == null ? (int?)null : int.Parse(value));
        AddParser(value => value?.Split(',').Select(int.Parse).ToList());
        AddParser(long.Parse);
        AddParser(value => value == null ? (long?)null : long.Parse(value));
        AddParser(value => value?.Split(',').Select(long.Parse).ToList());
        AddParser(ushort.Parse);
        AddParser(value => value == null ? (ushort?)null : ushort.Parse(value));
        AddParser(value => value?.Split(',').Select(ushort.Parse).ToList());
        AddParser(uint.Parse);
        AddParser(value => value == null ? (uint?)null : uint.Parse(value));
        AddParser(value => value?.Split(',').Select(uint.Parse).ToList());
        AddParser(ulong.Parse);
        AddParser(value => value == null ? (ulong?)null : ulong.Parse(value));
        AddParser(value => value?.Split(',').Select(ulong.Parse).ToList());
        AddParser(float.Parse);
        AddParser(value => value == null ? (float?)null : float.Parse(value));
        AddParser(value => value?.Split(',').Select(float.Parse).ToList());
        AddParser(double.Parse);
        AddParser(value => value == null ? (double?)null : double.Parse(value));
        AddParser(value => value?.Split(',').Select(double.Parse).ToList());
        AddParser(Guid.Parse);
        AddParser(value => value == null ? (Guid?)null : Guid.Parse(value));
        AddParser(value => value?.Split(',').Select(Guid.Parse).ToList());
        AddParser(TimeSpan.Parse);
        AddParser(value => value == null ? (TimeSpan?)null : TimeSpan.Parse(value));
        AddParser(value => value?.Split(',').Select(TimeSpan.Parse).ToList());
    }

    public static void AddParser<T>(Func<string, T> parseAction)
    {
        ParameterParser.Add(typeof(T), value => parseAction(value));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Command
{
    public abstract class GeneratorCommand<T> : IGeneratorCommand
        where T : GeneratorCommandParameters, new()
    {
        public abstract string[] Names { get; }
        public T Parameters { get; } = new T();
        GeneratorCommandParameters IGeneratorCommand.Parameters => this.Parameters;
        public List<RawCommandParameter> OriginalParameters { get; private set; }
        public List<ITransferObject> TransferObjects { get; set; }

        public bool Parse(params RawCommandParameter[] parameters)
        {
            return this.Parse(parameters.ToList());
        }

        public bool Parse(IEnumerable<RawCommandParameter> parameters)
        {
            this.OriginalParameters = parameters.ToList();
            Type type = typeof(T);
            Dictionary<string, PropertyInfo> mapping = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                bool isMapped = false;
                foreach (GeneratorParameterAttribute attribute in property.GetCustomAttributes<GeneratorParameterAttribute>())
                {
                    isMapped = true;
                    mapping.AddIfNotExists(RawCommandParameter.FormatName(attribute.ParameterName), property);
                }
                foreach (GeneratorGlobalParameterAttribute attribute in property.GetCustomAttributes<GeneratorGlobalParameterAttribute>().Where(x => !string.IsNullOrEmpty(x.ParameterName)))
                {
                    isMapped = true;
                    mapping.AddIfNotExists(RawCommandParameter.FormatName(attribute.ParameterName), property);
                }
                if (!isMapped)
                {
                    mapping.AddIfNotExists(RawCommandParameter.FormatName(property.Name), property);
                }
            }
            foreach (RawCommandParameter parameter in this.OriginalParameters)
            {
                string parameterName = parameter.Name.ToLowerInvariant();
                if (!mapping.ContainsKey(parameterName))
                {
                    Logger.Warning($"Unknown parameter '{parameter.Name}' on '{this.Names.First()}' command");
                    continue;
                }
                PropertyInfo property = mapping[parameterName];
                if (!property.CanWrite)
                {
                    Logger.Warning($"Can not write parameter '{parameter.Name}' on '{this.Names.First()}' command. Ensure command property has a public setter.");
                    continue;
                }

                if (!GeneratorCommand.ParameterParser.ContainsKey(property.PropertyType))
                {
                    Logger.Error($"Can not write parameter '{parameter.Name}' on '{this.Names.First()}' command. No converter from System.String to {property.PropertyType.FullName} found. Use {nameof(GeneratorCommand)}.{nameof(GeneratorCommand.ParameterParser)} to set a custom parser");
                    return false;
                }
                try
                {
                    property.SetMethod.Invoke(this.Parameters, new[]
                                                               {
                                                                   GeneratorCommand.ParameterParser[property.PropertyType].Invoke(parameter.Value)
                                                               });
                }
                catch (Exception exception)
                {
                    Logger.Error($"Can not set parameter '{this.Names.First()}.{parameter.Name}'. {exception.Message}");
                    return false;
                }
            }
            return true;
        }

        public abstract IGeneratorCommandResult Run(IOutput output);

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
            return $" {this.Names.First()} {string.Join(" ", this.OriginalParameters)}";
        }
    }

    public static class GeneratorCommand
    {
        internal static Dictionary<Type, Func<string, object>> ParameterParser { get; } = new Dictionary<Type, Func<string, object>>();

        static GeneratorCommand()
        {
            AddParser(value => value);
            AddParser(value => !value?.Equals(bool.FalseString) ?? true);
            AddParser(value => !value?.Equals(bool.FalseString));
            AddParser(byte.Parse);
            AddParser(value => value == null ? (byte?)null : byte.Parse(value));
            AddParser(char.Parse);
            AddParser(value => value == null ? (char?)null : char.Parse(value));
            AddParser(DateTime.Parse);
            AddParser(value => value == null ? (DateTime?)null : DateTime.Parse(value));
            AddParser(decimal.Parse);
            AddParser(value => value == null ? (decimal?)null : decimal.Parse(value));
            AddParser(short.Parse);
            AddParser(value => value == null ? (short?)null : short.Parse(value));
            AddParser(int.Parse);
            AddParser(value => value == null ? (int?)null : int.Parse(value));
            AddParser(long.Parse);
            AddParser(value => value == null ? (long?)null : long.Parse(value));
            AddParser(ushort.Parse);
            AddParser(value => value == null ? (ushort?)null : ushort.Parse(value));
            AddParser(uint.Parse);
            AddParser(value => value == null ? (uint?)null : uint.Parse(value));
            AddParser(ulong.Parse);
            AddParser(value => value == null ? (ulong?)null : ulong.Parse(value));
            AddParser(float.Parse);
            AddParser(value => value == null ? (float?)null : float.Parse(value));
            AddParser(double.Parse);
            AddParser(value => value == null ? (double?)null : double.Parse(value));
            AddParser(Guid.Parse);
            AddParser(value => value == null ? (Guid?)null : Guid.Parse(value));
            AddParser(TimeSpan.Parse);
            AddParser(value => value == null ? (TimeSpan?)null : TimeSpan.Parse(value));
        }

        public static void AddParser<T>(Func<string, T> parseAction)
        {
            ParameterParser.Add(typeof(T), value => parseAction(value));
        }
    }
}
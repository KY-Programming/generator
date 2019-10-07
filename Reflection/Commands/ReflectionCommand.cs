using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Reflection.Attributes;
using KY.Generator.Reflection.Configuration;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands
{
    internal class ReflectionCommand : IGeneratorCommand
    {
        private readonly ReflectionReader reader;
        private readonly ReflectionWriter writer;

        public string[] Names { get; } = { "reflection" };

        public ReflectionCommand(ReflectionReader reader, ReflectionWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }

        public bool Generate(CommandConfiguration configuration, IOutput output)
        {
            //Type type = reflectionConfiguration.GetType();
            //foreach (CommandParameter parameter in configuration.Parameters)
            //{
            //    PropertyInfo propertyInfo = type.GetProperty(parameter.Name);
            //    if (propertyInfo != null && propertyInfo.CanWrite)
            //    {
            //        propertyInfo.SetValue(reflectionConfiguration, );
            //    }
            //}
            if (configuration.Parameters.GetBool("fromAttributes") || configuration.Parameters.GetBool("fromAttribute"))
            {
                string assemblyName = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Assembly));
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    Assembly.LoadFile(assemblyName);
                }
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type objectType in this.GetTypes(assembly))
                    {
                        foreach (GenerateAttribute attribute in objectType.GetCustomAttributes<GenerateAttribute>())
                        {
                            ILanguage language = attribute.Language == OutputLanguage.Inherit ? configuration.Language : attribute.Language == OutputLanguage.Csharp ? (ILanguage)CsharpLanguage.Instance : TypeScriptLanguage.Instance;
                            if (!language.IsCsharp() && !language.IsTypeScript())
                            {
                                Logger.Error($"No language for type {objectType.Name} set. Set a language via attribute or use parameter (-language=Csharp)");
                                return false;
                            }

                            ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
                            readConfiguration.CopyBaseFrom(configuration);
                            readConfiguration.Language = language;
                            readConfiguration.Name = objectType.Name;
                            readConfiguration.Namespace = objectType.Namespace;
                            readConfiguration.Assembly = objectType.Assembly.Location;
                            List<ITransferObject> transferObjects = this.reader.Read(readConfiguration);

                            ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                            writeConfiguration.CopyBaseFrom(configuration);
                            writeConfiguration.Namespace = objectType.Namespace;
                            writeConfiguration.RelativePath = attribute.RelativePath ?? configuration.Parameters.GetString(nameof(ReflectionWriteConfiguration.RelativePath));
                            writeConfiguration.SkipNamespace = attribute.SkipNamespace.ToBool(configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.SkipNamespace)));
                            writeConfiguration.PropertiesToFields = attribute.PropertiesToFields.ToBool(configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.PropertiesToFields)));
                            writeConfiguration.FieldsToProperties = attribute.FieldsToProperties.ToBool(configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FieldsToProperties)));
                            writeConfiguration.FormatNames = attribute.FormatNames.ToBool(configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FormatNames)));
                            this.writer.Write(writeConfiguration, transferObjects, output);
                        }
                    }
                }
            }
            else
            {
                ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
                readConfiguration.CopyBaseFrom(configuration);
                readConfiguration.Name = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Name));
                readConfiguration.Namespace = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Namespace));
                readConfiguration.Assembly = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Assembly));
                List<ITransferObject> transferObjects = this.reader.Read(readConfiguration);
                
                ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                writeConfiguration.CopyBaseFrom(configuration);
                writeConfiguration.Namespace = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Namespace));
                writeConfiguration.RelativePath = configuration.Parameters.GetString(nameof(ReflectionWriteConfiguration.RelativePath));
                writeConfiguration.Using = configuration.Parameters.GetString(nameof(ReflectionWriteConfiguration.Using));
                writeConfiguration.SkipNamespace = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.SkipNamespace));
                writeConfiguration.PropertiesToFields = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.PropertiesToFields));
                writeConfiguration.FieldsToProperties = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FieldsToProperties));
                writeConfiguration.FormatNames = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FormatNames));
                this.writer.Write(writeConfiguration, transferObjects, output);
            }

            return true;
        }

        private IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                Logger.Error(exception);
                exception.LoaderExceptions.ForEach(Logger.Error);
                return Enumerable.Empty<Type>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
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
        private readonly GeneratorEnvironment environment;

        public string[] Names { get; } = { "reflection" };

        public ReflectionCommand(ReflectionReader reader, ReflectionWriter writer, GeneratorEnvironment environment)
        {
            this.reader = reader;
            this.writer = writer;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            string assemblyName = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Assembly));
            if (configuration.Parameters.GetBool("fromAttributes") || configuration.Parameters.GetBool("fromAttribute"))
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsAsync())
                    {
                        this.environment.SwitchToAsync = true;
                        continue;
                    }
                    foreach (Type objectType in TypeHelper.GetTypes(assembly))
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
                            readConfiguration.ReadFromParameters(configuration.Parameters);
                            readConfiguration.CopyBaseFrom(configuration);
                            readConfiguration.Language = language;
                            readConfiguration.Name = objectType.Name;
                            readConfiguration.Namespace = objectType.Namespace;
                            readConfiguration.Assembly = objectType.Assembly.Location;
                            readConfiguration.SkipSelf = attribute is GenerateIndexAttribute;
                            List<ITransferObject> transferObjects = new List<ITransferObject>();
                            this.reader.Read(readConfiguration, transferObjects);

                            ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                            writeConfiguration.ReadFromParameters(configuration.Parameters);
                            writeConfiguration.CopyBaseFrom(configuration);
                            writeConfiguration.Language = language;
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
                readConfiguration.Assembly = assemblyName;
                readConfiguration.SkipSelf = configuration.Parameters.GetBool(nameof(ReflectionReadConfiguration.SkipSelf));
                List<ITransferObject> transferObjects = new List<ITransferObject>();
                this.reader.Read(readConfiguration, transferObjects);

                ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                writeConfiguration.CopyBaseFrom(configuration);
                writeConfiguration.Language = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Language))?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false ? (ILanguage)CsharpLanguage.Instance : TypeScriptLanguage.Instance;
                writeConfiguration.Namespace = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Namespace));
                writeConfiguration.RelativePath = configuration.Parameters.GetString(nameof(ReflectionWriteConfiguration.RelativePath));
                writeConfiguration.Using = configuration.Parameters.GetString(nameof(ReflectionWriteConfiguration.Using));
                writeConfiguration.SkipNamespace = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.SkipNamespace), true);
                writeConfiguration.PropertiesToFields = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.PropertiesToFields), true);
                writeConfiguration.FieldsToProperties = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FieldsToProperties));
                writeConfiguration.FormatNames = configuration.Parameters.GetBool(nameof(ReflectionWriteConfiguration.FormatNames), true);
                this.writer.Write(writeConfiguration, transferObjects, output);
            }

            return true;
        }
    }
}
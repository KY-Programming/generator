using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands
{
    internal class ReflectionCommand : GeneratorCommand<ReflectionCommandParameters>
    {
        private readonly ReflectionReader reader;
        private readonly ReflectionWriter writer;

        public override string[] Names { get; } = { "reflection" };

        public ReflectionCommand(ReflectionReader reader, ReflectionWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            if (this.Parameters.FromAttributes)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsAsync() && !this.Parameters.IsAsync)
                    {
                        return this.SwitchAsync();
                    }
                    foreach (Type objectType in TypeHelper.GetTypes(assembly))
                    {
                        foreach (GenerateAttribute attribute in objectType.GetCustomAttributes<GenerateAttribute>())
                        {
                            ILanguage language = attribute.Language == OutputLanguage.Inherit ? this.Parameters.Language : attribute.Language == OutputLanguage.Csharp ? (ILanguage)CsharpLanguage.Instance : TypeScriptLanguage.Instance;
                            if (!language.IsCsharp() && !language.IsTypeScript())
                            {
                                Logger.Error($"No language for type {objectType.Name} set. Set a language via attribute or use parameter (-language=Csharp)");
                                return this.Error();
                            }

                            ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
                            readConfiguration.Language = language;
                            readConfiguration.Name = objectType.Name;
                            readConfiguration.Namespace = objectType.Namespace;
                            readConfiguration.Assembly = objectType.Assembly.Location;
                            readConfiguration.SkipSelf = attribute is GenerateIndexAttribute;
                            List<ITransferObject> transferObjects = new List<ITransferObject>();
                            this.reader.Read(readConfiguration, transferObjects);

                            ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                            writeConfiguration.AddHeader = !this.Parameters.SkipHeader;
                            writeConfiguration.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
                            writeConfiguration.Language = language;
                            writeConfiguration.Namespace = objectType.Namespace;
                            writeConfiguration.RelativePath = attribute.RelativePath ?? this.Parameters.RelativePath;
                            writeConfiguration.SkipNamespace = attribute.SkipNamespace.ToBool(this.Parameters.SkipNamespace);
                            writeConfiguration.PropertiesToFields = attribute.PropertiesToFields.ToBool(this.Parameters.PropertiesToFields);
                            writeConfiguration.FieldsToProperties = attribute.FieldsToProperties.ToBool(this.Parameters.FieldsToProperties);
                            writeConfiguration.FormatNames = attribute.FormatNames.ToBool(this.Parameters.FormatNames);

                            output.DeleteAllRelatedFiles(writeConfiguration.OutputId, this.Parameters.RelativePath);

                            this.writer.Write(writeConfiguration, transferObjects, output);
                        }
                    }
                }
            }
            else
            {
                ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
                readConfiguration.Name = this.Parameters.Name;
                readConfiguration.Namespace = this.Parameters.Namespace;
                readConfiguration.Assembly = this.Parameters.Assembly;
                readConfiguration.SkipSelf = this.Parameters.SkipSelf;
                List<ITransferObject> transferObjects = new List<ITransferObject>();
                this.reader.Read(readConfiguration, transferObjects);

                ReflectionWriteConfiguration writeConfiguration = new ReflectionWriteConfiguration();
                writeConfiguration.AddHeader = !this.Parameters.SkipHeader;
                writeConfiguration.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
                writeConfiguration.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false ? (ILanguage)CsharpLanguage.Instance : TypeScriptLanguage.Instance;
                writeConfiguration.Namespace = this.Parameters.Namespace;
                writeConfiguration.RelativePath = this.Parameters.RelativePath;
                writeConfiguration.Using = this.Parameters.Using;
                writeConfiguration.SkipNamespace = this.Parameters.SkipNamespace;
                writeConfiguration.PropertiesToFields = this.Parameters.PropertiesToFields;
                writeConfiguration.FieldsToProperties = this.Parameters.FieldsToProperties;
                writeConfiguration.FormatNames = this.Parameters.FormatNames;

                output.DeleteAllRelatedFiles(writeConfiguration.OutputId, this.Parameters.RelativePath);

                this.writer.Write(writeConfiguration, transferObjects, output);
            }

            return this.Success();
        }
    }
}
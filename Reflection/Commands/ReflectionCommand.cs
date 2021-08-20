using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Languages.Extensions;
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
    internal class ReflectionCommand : GeneratorCommand<ReflectionCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "reflection" };

        public ReflectionCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
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

                            DependencyResolver attributeResolver = new(this.resolver);
                            Options.Bind(attributeResolver);

                            IOptions options = attributeResolver.Get<Options>().Current;
                            options.SetFromParameter(this.Parameters);
                            options.SetOutputId(this.TransferObjects);
                            options.Language = language;
                            options.Formatting.FromLanguage(options.Language);

                            ReflectionReadConfiguration readConfiguration = new();
                            readConfiguration.Name = objectType.Name;
                            readConfiguration.Namespace = objectType.Namespace;
                            readConfiguration.Assembly = objectType.Assembly.Location;
                            readConfiguration.SkipSelf = attribute is GenerateIndexAttribute;

                            List<ITransferObject> transferObjects = new();
                            attributeResolver.Create<ReflectionReader>().Read(readConfiguration, transferObjects, options);
                            output.DeleteAllRelatedFiles(options.OutputId, this.Parameters.RelativePath);
                            string relativePath = attribute.RelativePath ?? this.Parameters.RelativePath;
                            attributeResolver.Create<ReflectionWriter>().Write(transferObjects, relativePath, output);
                        }
                    }
                }
            }
            else
            {
                DependencyResolver attributeResolver = new(this.resolver);
                Options.Bind(attributeResolver);

                IOptions options = attributeResolver.Get<Options>().Current;
                options.AddHeader = !this.Parameters.SkipHeader;
                options.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
                options.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false ? CsharpLanguage.Instance : TypeScriptLanguage.Instance;
                options.Formatting.FromLanguage(options.Language);
                options.SkipNamespace = this.Parameters.SkipNamespace;
                options.PropertiesToFields = this.Parameters.PropertiesToFields;
                options.FieldsToProperties = this.Parameters.FieldsToProperties;
                options.FormatNames = this.Parameters.FormatNames;

                ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
                readConfiguration.Name = this.Parameters.Name;
                readConfiguration.Namespace = this.Parameters.Namespace;
                readConfiguration.Assembly = this.Parameters.Assembly;
                readConfiguration.SkipSelf = this.Parameters.SkipSelf;

                List<ITransferObject> transferObjects = new();
                attributeResolver.Create<ReflectionReader>().Read(readConfiguration, transferObjects, options);
                output.DeleteAllRelatedFiles(options.OutputId, this.Parameters.RelativePath);
                attributeResolver.Create<ReflectionWriter>().Write(transferObjects, this.Parameters.RelativePath, output);
            }

            return this.Success();
        }
    }
}

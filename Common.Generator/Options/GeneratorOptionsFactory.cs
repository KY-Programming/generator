using System.Reflection;
using KY.Generator.Documentation;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator;

public class GeneratorOptionsFactory : IOptionsFactory
{
    public bool CanCreate(Type optionsType)
    {
        return optionsType == typeof(GeneratorOptions);
    }

    public object Create(Type optionsType, object key, object? parent, object global)
    {
        return new GeneratorOptions(parent as GeneratorOptions, global as GeneratorOptions, key);
    }

    public object CreateGlobal(Type optionsType, object key, object? parent)
    {
        return key switch
        {
            Assembly assembly => this.CreateFromCustomAttributes(assembly.GetCustomAttributes(), assembly, parent as GeneratorOptions),
            MemberInfo member => this.CreateFromCustomAttributes(member.GetCustomAttributes(), member, parent as GeneratorOptions),
            ParameterInfo parameter => this.CreateFromCustomAttributes(parameter.GetCustomAttributes(), parameter, parent as GeneratorOptions),
            Options.RootKey => new GeneratorOptions(parent as GeneratorOptions, null, "global"),
            _ => new GeneratorOptions(parent as GeneratorOptions, null, key)
            // _ => throw new InvalidOperationException($"Could not create {nameof(GeneratorOptions)} {key.GetType()}")
        };
    }

    private GeneratorOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, Assembly assembly, GeneratorOptions? parent)
    {
        GeneratorOptions options = this.CreateFromCustomAttributes(customAttributes, (object)assembly, parent);
        this.UpdateFromDocumentation(options, DocumentationReader.Get(assembly));
        return options;
    }

    private GeneratorOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, MemberInfo member, GeneratorOptions? parent)
    {
        GeneratorOptions options = this.CreateFromCustomAttributes(customAttributes, (object)member, parent);
        this.UpdateFromDocumentation(options, DocumentationReader.Get(member));
        return options;
    }

    private GeneratorOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, ParameterInfo parameter, GeneratorOptions? parent)
    {
        GeneratorOptions options = this.CreateFromCustomAttributes(customAttributes, (object)parameter, parent);
        this.UpdateFromDocumentation(options, DocumentationReader.Get(parameter));
        return options;
    }

    private GeneratorOptions CreateFromCustomAttributes(IEnumerable<Attribute> customAttributes, object key, GeneratorOptions? parent)
    {
        GeneratorOptions options = new(parent, null, key);
        foreach (Attribute attribute in customAttributes)
        {
            switch (attribute)
            {
                case GenerateIgnoreAttribute:
                    options.Ignore = true;
                    break;
                case GeneratePreferInterfacesAttribute:
                    options.PreferInterfaces = true;
                    break;
                case GeneratePropertiesAsFieldsAttribute:
                    options.FieldsToProperties = false;
                    options.PropertiesToFields = true;
                    break;
                case GenerateFieldsAsPropertiesAttribute:
                    options.FieldsToProperties = true;
                    options.PropertiesToFields = false;
                    break;
                case GenerateFormatNamesAttribute formatNamesAttribute:
                    options.FormatNames = formatNamesAttribute.FormatNames;
                    break;
                case GenerateNoHeaderAttribute:
                    options.AddHeader = false;
                    break;
                case GenerateOnlySubTypesAttribute:
                    options.OnlySubTypes = true;
                    break;
                case GenerateMemberAttribute memberAttribute:
                    if (!string.IsNullOrEmpty(memberAttribute.Name))
                    {
                        options.Rename = memberAttribute.Name;
                    }
                    if (memberAttribute.Type != null)
                    {
                        options.ReturnType = new TypeTransferObject
                        {
                            Name = memberAttribute.Type.Name,
                            Namespace = memberAttribute.Type.Namespace,
                            Type = memberAttribute.Type
                        };
                    }
                    else if (!string.IsNullOrEmpty(memberAttribute.TypeName))
                    {
                        options.ReturnType = new TypeTransferObject
                        {
                            Name = memberAttribute.TypeName
                        };
                    }
                    if (!string.IsNullOrEmpty(memberAttribute.Replace))
                    {
                        options.AddToReplaceName(memberAttribute.Replace!, memberAttribute.With ?? string.Empty);
                    }
                    break;
                case GenerateImportAttribute importAttribute:
                    options.AddToImports(new Import(importAttribute.Type, importAttribute.FileName, importAttribute.TypeName));
                    break;
                case GenerateNoOptionalAttribute:
                    options.NoOptional = true;
                    break;
                case GenerateModelOutputAttribute modelOutputAttribute:
                    options.ModelOutput = modelOutputAttribute.RelativePath;
                    break;
            }
        }
        return options;
    }

    private void UpdateFromDocumentation(GeneratorOptions options, string documentation)
    {
        if (documentation.Contains("GenerateIgnore") || documentation.Contains("Generator ignore"))
        {
            options.Ignore = true;
        }
    }
}

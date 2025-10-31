using System.Reflection;
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
            Assembly assembly => this.CreateFromCustomAttributes(assembly.GetCustomAttributes(), key, parent as GeneratorOptions),
            MemberInfo member => this.CreateFromCustomAttributes(member.GetCustomAttributes(), key, parent as GeneratorOptions),
            ParameterInfo parameter => this.CreateFromCustomAttributes(parameter.GetCustomAttributes(), key, parent as GeneratorOptions),
            Options.RootKey => new GeneratorOptions(parent as GeneratorOptions, null, "global"),
            _ => new GeneratorOptions(parent as GeneratorOptions, null, key)
            // _ => throw new InvalidOperationException($"Could not create {nameof(GeneratorOptions)} {key.GetType()}")
        };
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
                case GenerateRenameAttribute renameAttribute:
                    options.AddToReplaceName(renameAttribute.Replace, renameAttribute.With);
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
                case GenerateReturnTypeAttribute returnTypeAttribute:
                    if (returnTypeAttribute.Type != null)
                    {
                        options.ReturnType = new TypeTransferObject
                        {
                            Name = returnTypeAttribute.Type.Name,
                            Namespace = returnTypeAttribute.Type.Namespace,
                            Type = returnTypeAttribute.Type
                        };
                    }
                    else
                    {
                        options.ReturnType = new TypeTransferObject
                        {
                            Name = returnTypeAttribute.TypeName,
                            FileName = returnTypeAttribute.FileName,
                            OverrideType = returnTypeAttribute.OverrideName
                        };
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
}

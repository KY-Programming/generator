using System.Reflection;
using KY.Generator.Models;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection;

public class ReflectionOptionsReader : IGlobalOptionsReader
{
    public void Read(object key, OptionsSet entry)
    {
        if (key is not ICustomAttributeProvider attributeProvider)
        {
            return;
        }
        foreach (object attribute in attributeProvider.GetCustomAttributes(true))
        {
            switch (attribute)
            {
                case GenerateIgnoreAttribute:
                    entry.Part.Ignore = true;
                    break;
                case GeneratePreferInterfacesAttribute:
                    entry.Part.PreferInterfaces = true;
                    break;
                case GenerateStrictAttribute strictAttribute:
                    entry.Part.Strict = strictAttribute.Strict;
                    break;
                case GenerateRenameAttribute renameAttribute:
                    entry.Part.ReplaceName[renameAttribute.Replace] = renameAttribute.With;
                    break;
                case GeneratePropertiesAsFieldsAttribute:
                    entry.Part.FieldsToProperties = false;
                    entry.Part.PropertiesToFields = true;
                    break;
                case GenerateFieldsAsPropertiesAttribute:
                    entry.Part.FieldsToProperties = true;
                    entry.Part.PropertiesToFields = false;
                    break;
                case GenerateFormatNamesAttribute formatNamesAttribute:
                    entry.Part.FormatNames = formatNamesAttribute.FormatNames;
                    break;
                case GenerateNoHeaderAttribute:
                    entry.Part.AddHeader = false;
                    break;
                case GenerateOnlySubTypesAttribute:
                    entry.Part.OnlySubTypes = true;
                    break;
                case GenerateReturnTypeAttribute returnTypeAttribute:
                    if (returnTypeAttribute.Type != null)
                    {
                        entry.Part.ReturnType = new TypeTransferObject
                                                {
                                                    Name = returnTypeAttribute.Type.Name,
                                                    Namespace = returnTypeAttribute.Type.Namespace
                                                };
                    }
                    else
                    {
                        entry.Part.ReturnType = new TypeTransferObject
                                                {
                                                    Name = returnTypeAttribute.TypeName,
                                                    FileName = returnTypeAttribute.FileName,
                                                    OverrideType = returnTypeAttribute.OverrideName
                                                };
                    }
                    break;
                case GenerateImportAttribute importAttribute:
                    entry.Part.Imports.Add(new Import(importAttribute.Type, importAttribute.FileName, importAttribute.TypeName));
                    break;
            }
        }
    }
}

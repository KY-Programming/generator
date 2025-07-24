using System;
using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers;

public abstract class TransferWriter : Codeable
{
    protected ITypeMapping TypeMapping { get; }
    protected Options Options { get; }

    protected TransferWriter(ITypeMapping typeMapping, Options options)
    {
        this.TypeMapping = typeMapping;
        this.Options = options;
    }

    protected virtual void AddConstants(ModelTransferObject model, ClassTemplate classTemplate)
    {
        foreach (FieldTransferObject constant in model.Constants)
        {
            IOptions fieldOptions = this.Options.Get(constant);
            FieldTemplate fieldTemplate = this.AddField(model, constant, classTemplate).Constant();
            fieldTemplate.DefaultValue = this.ValueToTemplate(constant.Default, model, fieldOptions);
        }
    }

    protected virtual void AddFields(ModelTransferObject model, ClassTemplate classTemplate)
    {
        foreach (FieldTransferObject field in model.Fields)
        {
            IOptions fieldOptions = this.Options.Get(field);
            if (fieldOptions.FieldsToProperties)
            {
                this.AddProperty(model, field, classTemplate);
            }
            else
            {
                this.AddField(model, field, classTemplate);
            }
        }
    }

    protected virtual void AddProperties(ModelTransferObject model, ClassTemplate classTemplate)
    {
        foreach (PropertyTransferObject property in model.Properties)
        {
            IOptions propertyOptions = this.Options.Get(property);
            if (propertyOptions.PropertiesToFields)
            {
                this.AddField(model, property, classTemplate);
            }
            else
            {
                this.AddProperty(model, property, classTemplate);
            }
        }
    }

    protected virtual void MapType(ILanguage fromLanguage, ILanguage toLanguage, TypeTransferObject type)
    {
        if (type == null)
        {
            return;
        }
        this.TypeMapping.Get(fromLanguage, toLanguage, type);
        type.Generics.ForEach(x => this.MapType(fromLanguage, toLanguage, x.Type));
    }

    protected virtual FieldTemplate AddField(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        IOptions fieldOptions = this.Options.Get(member);
        if (model.Language != null && fieldOptions.Language != null)
        {
            this.MapType(model.Language, fieldOptions.Language, member.Type);
        }
        this.AddUsing(member.Type, classTemplate, fieldOptions);
        FieldTemplate fieldTemplate = classTemplate.AddField(member.Name, member.Type.ToTemplate()).Public().FormatName(fieldOptions)
                                                   .WithComment(member.Comment);
        fieldTemplate.IsOptional = member.IsOptional;
        fieldTemplate.DefaultValue = this.ValueToTemplate(member.Default, model, fieldOptions);
        if (fieldOptions.WithOptionalProperties)
        {
            fieldTemplate.Optional();
        }
        return fieldTemplate;
    }

    protected virtual PropertyTemplate AddProperty(ModelTransferObject model, MemberTransferObject member, ClassTemplate classTemplate)
    {
        bool canRead = true;
        bool canWrite = true;
        if (member is PropertyTransferObject property)
        {
            canRead = property.CanRead;
            canWrite = property.CanWrite;
        }
        IOptions propertyOptions = this.Options.Get(member);
        if (model.Language != null && propertyOptions.Language != null)
        {
            this.MapType(model.Language, propertyOptions.Language, member.Type);
        }
        PropertyTemplate propertyTemplate = classTemplate.AddProperty(member.Name, member.Type.ToTemplate()).FormatName(propertyOptions);
        propertyTemplate.HasGetter = canRead;
        propertyTemplate.HasSetter = canWrite;
        propertyTemplate.IsOptional = member.IsOptional;
        propertyTemplate.DefaultValue = this.ValueToTemplate(member.Default, model, propertyOptions);
        if (propertyOptions.WithOptionalProperties)
        {
            propertyTemplate.Optional();
        }
        if (member.Type != model)
        {
            this.AddUsing(member.Type, classTemplate, propertyOptions);
        }
        return propertyTemplate;
    }

    protected virtual void AddUsing(TypeTransferObject type, ClassTemplate classTemplate, IOptions options, string relativeModelPath = "./")
    {
        Import import = options.Imports.FirstOrDefault(import => import.Type.Namespace == type.Namespace && import.Type.Name == type.Name);
        if (import != null)
        {
            classTemplate.AddUsing(null, import.TypeName, import.FileName);
            return;
        }
        if (type is ModelTransferObject model)
        {
            foreach (GenericAliasTransferObject generic in model.Generics.Where(x => x.Type != null))
            {
                this.AddUsing(generic.Type, classTemplate, options, relativeModelPath);
            }
        }
        string importPath = relativeModelPath.Replace("\\", "/").TrimEnd('/');
        if ((!type.FromSystem || (type.FromSystem && options.Language.ImportFromSystem))
            && !string.IsNullOrEmpty(type.Namespace)
            && classTemplate.Namespace.Name != type.Namespace
            && !type.IsGenericParameter)
        {
            classTemplate.AddUsing(type, importPath);
        }
        if ((!type.FromSystem || (type.FromSystem && options.Language.ImportFromSystem))
            && !string.IsNullOrEmpty(type.FileName)
            && !type.IsGenericParameter)
        {
            classTemplate.AddUsing(type, importPath);
        }
        type.Generics.Where(x => x.Alias == null).ForEach(generic => this.AddUsing(generic.Type, classTemplate, options, relativeModelPath));
    }

    protected virtual ICodeFragment ValueToTemplate<T>(T value, ModelTransferObject model, IOptions memberOptions)
    {
        if (value == null)
        {
            return null;
        }
        if (value is TypeTransferObject typeTransferObject)
        {
            this.MapType(model.Language, memberOptions.Language, typeTransferObject);
            if (typeTransferObject.Name == "Array")
            {
                return Code.Local("[]");
            }
            return Code.New(typeTransferObject.ToTemplate());
        }
        Type valueType = value.GetType();
        if (valueType.IsEnum)
        {
            TypeTemplate type = Code.Type(Formatter.FormatClass(valueType.Name, memberOptions));
            string property = Formatter.FormatField(value.ToString(), memberOptions);
            return Code.Static(type).Property(property);
        }
        switch (value)
        {
            case int intValue:
                return Code.Number(intValue);
            case long longValue:
                return Code.Number(longValue);
            case short shortValue:
                return Code.Number(shortValue);
            case uint uintValue:
                return Code.Number(uintValue);
            case ulong ulongValue:
                return Code.Number(ulongValue);
            case ushort ushortValue:
                return Code.Number(ushortValue);
            case float floatValue:
                return Code.Number(floatValue);
            case double doubleValue:
                return Code.Number(doubleValue);
            case DateTime dateTimeValue:
                return Code.DateTime(dateTimeValue);
            case bool boolValue:
                return Code.Boolean(boolValue);
            case byte byteValue:
                return Code.Number(byteValue);
            case sbyte sbyteValue:
                return Code.Number(sbyteValue);
            default:
                return Code.String(value?.ToString());
        }
    }
}

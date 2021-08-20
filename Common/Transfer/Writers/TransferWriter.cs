﻿using System;
using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers
{
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
                FieldTemplate fieldTemplate = this.AddField(model, constant, classTemplate).Constant();
                if (constant.Default != null)
                {
                    Type type = constant.Default.GetType();
                    if (type == typeof(int))
                    {
                        fieldTemplate.DefaultValue = Code.Number((int)constant.Default);
                    }
                    else if (type == typeof(long))
                    {
                        fieldTemplate.DefaultValue = Code.Number((long)constant.Default);
                    }
                    else if (type == typeof(short))
                    {
                        fieldTemplate.DefaultValue = Code.Number((short)constant.Default);
                    }
                    else if (type == typeof(uint))
                    {
                        fieldTemplate.DefaultValue = Code.Number((uint)constant.Default);
                    }
                    else if (type == typeof(ulong))
                    {
                        fieldTemplate.DefaultValue = Code.Number((ulong)constant.Default);
                    }
                    else if (type == typeof(ushort))
                    {
                        fieldTemplate.DefaultValue = Code.Number((ushort)constant.Default);
                    }
                    else if (type == typeof(float))
                    {
                        fieldTemplate.DefaultValue = Code.Number((float)constant.Default);
                    }
                    else if (type == typeof(double))
                    {
                        fieldTemplate.DefaultValue = Code.Number((double)constant.Default);
                    }
                    else if (type == typeof(DateTime))
                    {
                        fieldTemplate.DefaultValue = Code.DateTime((DateTime)constant.Default);
                    }
                    else if (type == typeof(bool))
                    {
                        fieldTemplate.DefaultValue = Code.Boolean((bool)constant.Default);
                    }
                    else
                    {
                        fieldTemplate.DefaultValue = Code.String(constant.Default.ToString());
                    }
                }
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

        protected void MapType(ILanguage fromLanguage, ILanguage toLanguage, TypeTransferObject type)
        {
            if (fromLanguage is IMappableLanguage mappableFromLanguage && toLanguage is IMappableLanguage mappableToLanguage)
            {
                this.MapType(mappableFromLanguage, mappableToLanguage, type);
            }
        }

        protected virtual void MapType(IMappableLanguage fromLanguage, IMappableLanguage toLanguage, TypeTransferObject type)
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
            if (model.Language is IMappableLanguage modelLanguage && fieldOptions.Language is IMappableLanguage configurationLanguage)
            {
                this.MapType(modelLanguage, configurationLanguage, member.Type);
            }
            this.AddUsing(member.Type, classTemplate, fieldOptions);
            FieldTemplate fieldTemplate = classTemplate.AddField(member.Name, member.Type.ToTemplate()).Public().FormatName(fieldOptions)
                                                       .WithComment(member.Comment);
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
            if (model.Language is IMappableLanguage modelLanguage && propertyOptions.Language is IMappableLanguage configurationLanguage)
            {
                this.MapType(modelLanguage, configurationLanguage, member.Type);
            }
            PropertyTemplate propertyTemplate = classTemplate.AddProperty(member.Name, member.Type.ToTemplate()).FormatName(propertyOptions);
            propertyTemplate.HasGetter = canRead;
            propertyTemplate.HasSetter = canWrite;
            if (propertyOptions.WithOptionalProperties)
            {
                propertyTemplate.Optional();
            }
            this.AddUsing(member.Type, classTemplate, propertyOptions);
            return propertyTemplate;
        }

        protected virtual void AddUsing(TypeTransferObject type, ClassTemplate classTemplate, IOptions options, string relativeModelPath = "./")
        {
            if (type.Name == classTemplate.Name)
            {
                return;
            }
            if (type is ModelTransferObject model)
            {
                foreach (GenericAliasTransferObject generic in model.Generics)
                {
                    this.AddUsing(generic.Type, classTemplate, options, relativeModelPath);
                }
            }
            if ((!type.FromSystem || type.FromSystem && options.Language.ImportFromSystem) && type.HasUsing && !string.IsNullOrEmpty(type.Namespace) && classTemplate.Namespace.Name != type.Namespace)
            {
                Func<string> action = () =>
                {
                    string fileName = options.Language is IFormattableLanguage formattableLanguage
                                          ? formattableLanguage.FormatFileName(type.Name)
                                          : Formatter.FormatFile(type.Name, options, true);
                    return $"{relativeModelPath.Replace("\\", "/").TrimEnd('/')}/{fileName}";
                };
                classTemplate.AddUsing(type, action);
            }
            type.Generics.Where(x => x.Alias == null).ForEach(generic => this.AddUsing(generic.Type, classTemplate, options, relativeModelPath));
        }
    }
}
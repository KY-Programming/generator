using System;
using System.Linq;
using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers
{
    public abstract class TransferWriter : Codeable
    {
        protected ITypeMapping TypeMapping { get; }

        public TransferWriter(ITypeMapping typeMapping)
        {
            this.TypeMapping = typeMapping;
        }

        protected virtual void AddConstants(ModelTransferObject model, ClassTemplate classTemplate, IConfiguration configuration)
        {
            foreach (FieldTransferObject constant in model.Constants)
            {
                FieldTemplate fieldTemplate = this.AddField(model, constant.Name, constant.Type, classTemplate, configuration).Constant();
                if (constant.Default != null)
                {
                    Type type = constant.Default.GetType();
                    if (type == typeof(int))
                    {
                        fieldTemplate.DefaultValue = Code.Number((int)constant.Default);
                    }
                    else  if (type == typeof(long))
                    {
                        fieldTemplate.DefaultValue = Code.Number((long)constant.Default);
                    }
                    else  if (type == typeof(short))
                    {
                        fieldTemplate.DefaultValue = Code.Number((short)constant.Default);
                    }
                    else  if (type == typeof(uint))
                    {
                        fieldTemplate.DefaultValue = Code.Number((uint)constant.Default);
                    }
                    else  if (type == typeof(ulong))
                    {
                        fieldTemplate.DefaultValue = Code.Number((ulong)constant.Default);
                    }
                    else  if (type == typeof(ushort))
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

        protected virtual void AddFields(ModelTransferObject model, ClassTemplate classTemplate, IConfiguration configuration)
        {
            foreach (FieldTransferObject field in model.Fields)
            {
                if (configuration.Formatting.FieldsToProperties)
                {
                    this.AddProperty(model, field.Name, field.Type, classTemplate, configuration);
                }
                else
                {
                    this.AddField(model, field.Name, field.Type, classTemplate, configuration);
                }
            }
        }

        protected virtual void AddProperties(ModelTransferObject model, ClassTemplate classTemplate, IConfiguration configuration)
        {
            foreach (PropertyTransferObject property in model.Properties)
            {
                if (configuration.Formatting.PropertiesToFields)
                {
                    this.AddField(model, property.Name, property.Type, classTemplate, configuration);
                }
                else
                {
                    this.AddProperty(model, property.Name, property.Type, classTemplate, configuration, property.CanRead, property.CanWrite);
                }
            }
        }

        protected virtual void MapType(IMappableLanguage fromLanguage, IMappableLanguage toLanguage, TypeTransferObject type)
        {
            this.TypeMapping.Get(fromLanguage, toLanguage, type);
            type.Generics.ForEach(x => this.MapType(fromLanguage, toLanguage, x.Type));
        }

        protected virtual FieldTemplate AddField(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration)
        {
            if (model.Language is IMappableLanguage modelLanguage && configuration.Language is IMappableLanguage configurationLanguage)
            {
                this.MapType(modelLanguage, configurationLanguage, type);
            }
            this.AddUsing(type, classTemplate, configuration);
            return classTemplate.AddField(name, type.ToTemplate()).Public().FormatName(configuration);
        }

        protected virtual PropertyTemplate AddProperty(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration, bool canRead = true, bool canWrite = true)
        {
            if (model.Language is IMappableLanguage modelLanguage && configuration.Language is IMappableLanguage configurationLanguage)
            {
                this.MapType(modelLanguage, configurationLanguage, type);
            }
            PropertyTemplate propertyTemplate = classTemplate.AddProperty(name, type.ToTemplate()).FormatName(configuration);
            propertyTemplate.HasGetter = canRead;
            propertyTemplate.HasSetter = canWrite;
            this.AddUsing(type, classTemplate, configuration);
            return propertyTemplate;
        }

        protected virtual void AddUsing(TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration, string relativeModelPath = "./")
        {
            if (type.Name == classTemplate.Name)
            {
                return;
            }
            if ((!type.FromSystem || type.FromSystem && configuration.Language.ImportFromSystem) && !string.IsNullOrEmpty(type.Namespace) && classTemplate.Namespace.Name != type.Namespace)
            {
                string fileName = Formatter.FormatFile(type.Name, configuration, true);
                classTemplate.AddUsing(type.Namespace, type.Name, $"{relativeModelPath.Replace("\\", "/").TrimEnd('/')}/{fileName}");
            }
            type.Generics.Where(x => x.Alias == null).ForEach(generic => this.AddUsing(generic.Type, classTemplate, configuration, relativeModelPath));
        }
    }
}
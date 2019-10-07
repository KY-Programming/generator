using KY.Generator.Configurations;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer
{
    public abstract class TransferWriter : Codeable
    {
        protected ITypeMapping TypeMapping { get; }

        public TransferWriter(ITypeMapping typeMapping)
        {
            this.TypeMapping = typeMapping;
        }

        protected virtual void AddFields(ModelTransferObject model, ClassTemplate classTemplate, IFormattableConfiguration configuration)
        {
            foreach (ModelFieldTransferObject field in model.Fields)
            {
                if (configuration.FieldsToProperties)
                {
                    this.AddProperty(model, field.Name, field.Type, classTemplate, configuration);
                }
                else
                {
                    this.AddField(model, field.Name, field.Type, classTemplate, configuration);
                }
            }
        }

        protected virtual void AddProperties(ModelTransferObject model, ClassTemplate classTemplate, IFormattableConfiguration configuration)
        {
            foreach (ModelPropertyTransferObject property in model.Properties)
            {
                if (configuration.PropertiesToFields)
                {
                    this.AddField(model, property.Name, property.Type, classTemplate, configuration);
                }
                else
                {
                    this.AddProperty(model, property.Name, property.Type, classTemplate, configuration, property.CanRead, property.CanWrite);
                }
            }
        }

        protected virtual void MapType(ILanguage fromLanguage, ILanguage toLanguage, TypeTransferObject type)
        {
            this.TypeMapping.Get(fromLanguage, toLanguage, type);
            type.Generics.ForEach(x => this.MapType(fromLanguage, toLanguage, x));
        }

        protected virtual void AddField(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IFormattableConfiguration configuration)
        {
            this.MapType(model.Language, configuration.Language, type);
            name = configuration.FormatNames && configuration.Language is IFormattableLanguage formattableLanguage ? formattableLanguage.FormatFieldName(name) : name;
            classTemplate.AddField(name, type.ToTemplate()).Public().FormatName(configuration.Language, configuration.FormatNames);
            this.AddUsing(type, classTemplate, configuration.Language);
        }

        protected virtual void AddProperty(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IFormattableConfiguration configuration, bool canRead = true, bool canWrite = true)
        {
            this.MapType(model.Language, configuration.Language, type);
            name = configuration.FormatNames && configuration.Language is IFormattableLanguage formattableLanguage ? formattableLanguage.FormatPropertyName(name) : name;
            PropertyTemplate propertyTemplate = classTemplate.AddProperty(name, type.ToTemplate()).FormatName(configuration.Language, configuration.FormatNames);
            propertyTemplate.HasGetter = canRead;
            propertyTemplate.HasSetter = canWrite;
            this.AddUsing(type, classTemplate, configuration.Language);
        }

        protected virtual void AddUsing(TypeTransferObject type, ClassTemplate classTemplate, ILanguage language, string relativeModelPath = "./")
        {
            if (type.Name == classTemplate.Name)
            {
                return;
            }
            if ((!type.FromSystem || type.FromSystem && language.ImportFromSystem) && !string.IsNullOrEmpty(type.Namespace) && classTemplate.Namespace.Name != type.Namespace)
            {
                string fileName = language is IFormattableLanguage formattableLanguage ? formattableLanguage.FormatFileName(type.Name, false) : type.Name;
                classTemplate.AddUsing(type.Namespace, type.Name, $"{relativeModelPath.Replace("\\", "/").TrimEnd('/')}/{fileName}");
            }
            type.Generics.ForEach(generic => this.AddUsing(generic, classTemplate, language, relativeModelPath));
        }

        //private string GetTypePath(Type type, IModelConfiguration configuration)
        //{
        //    return configuration.Types.FirstOrDefault(x => x.Name == type.Name)?.Using ?? "./" + Code.GetFileName(type.Name);
        //}
    }
}
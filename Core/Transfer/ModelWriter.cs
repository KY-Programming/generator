using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer
{
    public class ModelWriter : TransferWriter, ITransferWriter
    {
        public ModelWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        public virtual void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            IModelConfiguration configuration = (IModelConfiguration)configurationBase;
            this.Write(configuration, transferObjects).ForEach(file => configuration.Language.Write(file, output));
        }

        public virtual List<FileTemplate> Write(IModelConfiguration configuration, IEnumerable<ITransferObject> transferObjects)
        {
            if (configuration.Language == null)
            {
                throw new InvalidOperationException("Can not write model without language");
            }
            List<FileTemplate> files = new List<FileTemplate>();
            List<ModelTransferObject> models = transferObjects.OfType<ModelTransferObject>().ToList();
            if (!string.IsNullOrEmpty(configuration.Name))
            {
                models.Where(x => x.Name == "Unknown").ForEach(x => x.Name = configuration.Name);
            }
            if (configuration.FormatNames && configuration.Language is IFormattableLanguage formattableLanguage)
            {
                models.ForEach(model => model.Name = formattableLanguage.FormatClassName(model.Name));
            }
            foreach (ModelTransferObject model in models)
            {
                string nameSpace = configuration.SkipNamespace ? string.Empty : configuration.Namespace ?? model.Namespace;
                this.WriteModel(configuration, model, nameSpace, files);
            }
            return files;
        }

        protected virtual void WriteModel(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            if (model == null)
            {
                return;
            }
            this.MapType(model.Language, configuration.Language, model);
            if (model.FromSystem)
            {
                return;
            }
            if (model.IsEnum)
            {
                this.WriteEnum(configuration, model, nameSpace, files);
            }
            else
            {
                this.WriteClass(configuration, model, nameSpace, files);
            }
        }

        protected virtual EnumTemplate WriteEnum(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            if (model.EnumValues == null)
            {
                throw new InvalidOperationException("Can not write enum without values");
            }

            EnumTemplate enumTemplate = files.AddFile(configuration.RelativePath, configuration.AddHeader)
                                             .AddNamespace(nameSpace)
                                             .AddEnum(model.Name);

            foreach (KeyValuePair<string, int> pair in model.EnumValues)
            {
                string formattedName = configuration.FormatNames && configuration.Language is IFormattableLanguage formattableLanguage ? formattableLanguage.FormatPropertyName(pair.Key) : pair.Key;
                enumTemplate.Values.Add(new EnumValueTemplate(pair.Key, Code.Number(pair.Value), formattedName));
            }
            return enumTemplate;
        }

        protected virtual ClassTemplate WriteClass(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            if (model.BasedOn != null)
            {
                this.MapType(model.Language, configuration.Language, model.BasedOn);
            }

            ClassTemplate classTemplate = files.AddFile(configuration.RelativePath, configuration.AddHeader)
                                               .AddNamespace(nameSpace)
                                               .AddClass(model.Name, model.BasedOn?.ToTemplate());

            if (model.BasedOn != null)
            {
                this.AddUsing(model.BasedOn, classTemplate, configuration.Language);
            }

            classTemplate.IsInterface = model.IsInterface;
            classTemplate.IsAbstract = model.IsAbstract;
            if (model.IsGeneric)
            {
                model.Generics.ForEach(x => this.AddUsing(x, classTemplate, configuration.Language));
                classTemplate.Generics.AddRange(model.Generics.Select(x => new ClassGenericTemplate(x.Name)));
            }
            foreach (TypeTransferObject interFace in model.Interfaces)
            {
                this.MapType(model.Language, configuration.Language, interFace);
                classTemplate.BasedOn.Add(new BaseTypeTemplate(classTemplate, Code.Interface(interFace.Name, interFace.Namespace)));
                this.AddUsing(interFace, classTemplate, configuration.Language);
            }
            this.AddFields(model, classTemplate, configuration);
            this.AddProperties(model, classTemplate, configuration);
            return classTemplate;
        }
    }
}
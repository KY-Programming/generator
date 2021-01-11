using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers
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
            models.ForEach(model => model.Name = Formatter.FormatClass(model.Name, configuration));
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
            if (model.Language is IMappableLanguage modelLanguage && configuration.Language is IMappableLanguage configurationLanguage)
            {
                this.MapType(modelLanguage, configurationLanguage, model);
            }
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

            EnumTemplate enumTemplate = files.AddFile(configuration.RelativePath, configuration.AddHeader, configuration.OutputId)
                                             .AddNamespace(nameSpace)
                                             .AddEnum(model.Name);

            foreach (KeyValuePair<string, int> pair in model.EnumValues)
            {
                string formattedName = Formatter.FormatProperty(pair.Key, configuration);
                enumTemplate.Values.Add(new EnumValueTemplate(pair.Key, Code.Number(pair.Value), formattedName));
            }
            return enumTemplate;
        }

        protected virtual ClassTemplate WriteClass(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            IMappableLanguage modelLanguage = model.Language as IMappableLanguage;
            IMappableLanguage configurationLanguage = configuration.Language as IMappableLanguage;

            if (model.BasedOn != null && modelLanguage != null && configurationLanguage != null)
            {
                this.MapType(modelLanguage, configurationLanguage, model.BasedOn);
            }

            ClassTemplate classTemplate = files.AddFile(configuration.RelativePath, configuration.AddHeader, configuration.OutputId)
                                               .WithType(model.IsInterface ? "interface" : null)
                                               .AddNamespace(nameSpace)
                                               .AddClass(model.Name, model.BasedOn?.ToTemplate())
                                               .FormatName(configuration);

            if (model.BasedOn != null)
            {
                this.AddUsing(model.BasedOn, classTemplate, configuration);
            }
            configuration.Usings?.ForEach(x => classTemplate.AddUsing(x, null, null));

            classTemplate.IsInterface = model.IsInterface;
            classTemplate.IsAbstract = model.IsAbstract;
            if (model.IsGeneric)
            {
                classTemplate.Generics.AddRange(model.Generics.Where(x => x.Alias != null).Select(x => new ClassGenericTemplate(x.Alias)));
            }
            foreach (TypeTransferObject interFace in model.Interfaces)
            {
                if (modelLanguage != null && configurationLanguage != null)
                {
                    this.MapType(modelLanguage, configurationLanguage, interFace);
                }
                classTemplate.BasedOn.Add(new BaseTypeTemplate(classTemplate, Code.Interface(interFace.Name, interFace.Namespace)));
                this.AddUsing(interFace, classTemplate, configuration);
            }
            this.AddConstants(model, classTemplate, configuration);
            this.AddFields(model, classTemplate, configuration);
            this.AddProperties(model, classTemplate, configuration);
            return classTemplate;
        }
    }
}
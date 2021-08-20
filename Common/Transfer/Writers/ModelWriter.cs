using System;
using System.Collections.Generic;
using System.Linq;
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
        public ModelWriter(ITypeMapping typeMapping, Options options)
            : base(typeMapping, options)
        { }

        public virtual void Write(IEnumerable<ITransferObject> transferObjects, string relativePath, IOutput output)
        {
            List<ModelTransferObject> models = transferObjects.OfType<ModelTransferObject>().ToList();
            foreach (ModelTransferObject model in models)
            {
                model.Name = Formatter.FormatClass(model.Name, this.Options.Get(model));
            }
            List<FileTemplate> files = new();
            foreach (ModelTransferObject model in models)
            {
                this.WriteModel(model, relativePath, files);
            }
            files.ForEach(file => this.Options.Current.Language.Write(file, output));
        }

        protected virtual void WriteModel(ModelTransferObject model, string relativePath, List<FileTemplate> files)
        {
            IOptions modelOptions = this.Options.Get(model);
            if (modelOptions.Language == null)
            {
                throw new InvalidOperationException("Can not write model without language");
            }
            if (model.Language is IMappableLanguage modelLanguage && modelOptions.Language is IMappableLanguage outputLanguage)
            {
                this.MapType(modelLanguage, outputLanguage, model);
            }
            if (model.FromSystem)
            {
                return;
            }
            if (model.IsEnum)
            {
                this.WriteEnum(model, relativePath, files);
            }
            else
            {
                this.WriteClass(model, relativePath, files);
            }
        }

        protected virtual EnumTemplate WriteEnum(ModelTransferObject model, string relativePath, List<FileTemplate> files)
        {
            if (model.EnumValues == null)
            {
                throw new InvalidOperationException("Can not write enum without values");
            }
            IOptions modelOptions = this.Options.Get(model);
            EnumTemplate enumTemplate = files.AddFile(relativePath, modelOptions.AddHeader, modelOptions.OutputId)
                                             .AddNamespace(modelOptions.SkipNamespace ? string.Empty : model.Namespace)
                                             .AddEnum(model.Name);

            foreach (KeyValuePair<string, int> pair in model.EnumValues)
            {
                string formattedName = Formatter.FormatProperty(pair.Key, modelOptions);
                enumTemplate.Values.Add(new EnumValueTemplate(pair.Key, Code.Number(pair.Value), formattedName));
            }
            return enumTemplate;
        }

        protected virtual ClassTemplate WriteClass(ModelTransferObject model, string relativePath, List<FileTemplate> files)
        {
            IOptions modelOptions = this.Options.Get(model);
            IMappableLanguage modelLanguage = model.Language as IMappableLanguage;
            IMappableLanguage outputLanguage = modelOptions.Language as IMappableLanguage;

            if (model.BasedOn != null && modelLanguage != null && outputLanguage != null)
            {
                this.MapType(modelLanguage, outputLanguage, model.BasedOn);
            }

            bool isInterface = model.IsInterface || modelOptions.PreferInterfaces;
            string modelNamespace = modelOptions.SkipNamespace ? string.Empty : model.Namespace;
            ClassTemplate otherClassTemplate = files.SelectMany(file => file.Namespaces)
                                                    .SelectMany(ns => ns.Children).OfType<ClassTemplate>()
                                                    .FirstOrDefault(x => x.Namespace.Name == modelNamespace && x.Name == model.Name);
            NamespaceTemplate namespaceTemplate = modelOptions.Language.IsGenericTypeWithSameNameAllowed ? otherClassTemplate?.Namespace : null;
            namespaceTemplate ??= files.AddFile(relativePath, modelOptions.AddHeader, modelOptions.OutputId)
                                       // .WithType(isInterface ? "interface" : null)
                                       .AddNamespace(modelNamespace);

            ClassTemplate classTemplate = namespaceTemplate.AddClass(model.Name, model.BasedOn?.ToTemplate()).FormatName(modelOptions);

            if (!modelOptions.Language.IsGenericTypeWithSameNameAllowed && otherClassTemplate != null)
            {
                if (model.IsGeneric)
                {
                    model.Name += "Generic";
                    classTemplate.Name += "Generic";
                }
                else
                {
                    model.Name += "NonGeneric";
                    classTemplate.Name += "NonGeneric";
                }
            }

            if (model.BasedOn != null)
            {
                this.AddUsing(model.BasedOn, classTemplate, modelOptions);
            }

            classTemplate.IsInterface = isInterface;
            classTemplate.IsAbstract = model.IsAbstract;
            if (model.IsGeneric)
            {
                classTemplate.Generics.AddRange(model.Generics.Where(x => x.Alias != null).Select(x => new ClassGenericTemplate(x.Alias.Name)));
            }
            foreach (TypeTransferObject interFace in model.Interfaces)
            {
                if (modelLanguage != null && outputLanguage != null)
                {
                    this.MapType(modelLanguage, outputLanguage, interFace);
                }
                classTemplate.BasedOn.Add(new BaseTypeTemplate(classTemplate, interFace.ToTemplate()));
                this.AddUsing(interFace, classTemplate, modelOptions);
            }
            this.AddConstants(model, classTemplate);
            this.AddFields(model, classTemplate);
            this.AddProperties(model, classTemplate);
            return classTemplate;
        }
    }
}

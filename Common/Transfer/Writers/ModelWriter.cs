using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers
{
    public class ModelWriter : TransferWriter, ITransferWriter
    {
        private readonly IEnumerable<ITransferObject> transferObjects;
        private readonly IList<FileTemplate> files;

        public ModelWriter(ITypeMapping typeMapping, Options options, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
            : base(typeMapping, options)
        {
            this.transferObjects = transferObjects;
            this.files = files;
        }

        public void FormatNames()
        {
            List<ModelTransferObject> models = this.transferObjects.OfType<ModelTransferObject>().ToList();
            foreach (ModelTransferObject model in models)
            {
                IOptions modelOptions = this.Options.Get(model);
                model.Name = Formatter.FormatClass(model.OriginalName, modelOptions)
                                      .Replace(modelOptions.ReplaceName)
                                      .Prefix(model.IsInterface || modelOptions.PreferInterfaces ? modelOptions.Formatting.InterfacePrefix : modelOptions.Formatting.ClassPrefix);
                if (!modelOptions.Language.IsGenericTypeWithSameNameAllowed)
                {
                    ModelTransferObject otherModel = models.FirstOrDefault(m => m != model
                                                                                && m.Name == model.Name
                                                                                && m.Namespace == model.Namespace
                                                                                && (m as GenericModelTransferObject)?.Template != (model as GenericModelTransferObject)?.Template
                    );
                    if (otherModel != null && otherModel.IsGeneric && !otherModel.FromSystem)
                    {
                        IOptions otherModelOptions = this.Options.Get(otherModel);
                        otherModel.Name = Formatter.FormatClass(otherModel.Name + "Generic", otherModelOptions)
                                                   .Replace(modelOptions.ReplaceName);
                        otherModel.FileName = Formatter.FormatFile(otherModel.Name, otherModelOptions);
                    }
                    else if (otherModel != null && model.IsGeneric && !otherModel.FromSystem)
                    {
                        model.Name = Formatter.FormatClass(model.Name + "Generic", modelOptions)
                                              .Replace(modelOptions.ReplaceName);
                    }
                }
                model.FileName = Formatter.FormatFile(model.Name, modelOptions);
            }
        }

        public virtual void Write(string relativePath)
        {
            foreach (ModelTransferObject model in this.transferObjects.OfType<ModelTransferObject>())
            {
                this.WriteModel(model, relativePath);
            }
        }

        protected virtual void WriteModel(ModelTransferObject model, string relativePath)
        {
            if (this.files.Any(file => file.Name == model.FileName && file.RelativePath == relativePath))
            {
                return;
            }
            IOptions modelOptions = this.Options.Get(model);
            if (modelOptions.OnlySubTypes)
            {
                Logger.Trace($"{model.Name} ({model.Namespace}) skipped through configuration");
                return;
            }
            if (modelOptions.Language == null)
            {
                throw new InvalidOperationException("Can not write model without language");
            }
            if (model.Language != null && modelOptions.Language != null)
            {
                this.MapType(model.Language, modelOptions.Language, model);
            }
            if (model.FromSystem)
            {
                return;
            }
            if (model.IsEnum)
            {
                this.WriteEnum(model, relativePath);
            }
            else
            {
                this.WriteClass(model, relativePath);
            }
        }

        protected virtual EnumTemplate WriteEnum(ModelTransferObject model, string relativePath)
        {
            if (model.EnumValues == null)
            {
                throw new InvalidOperationException("Can not write enum without values");
            }
            IOptions modelOptions = this.Options.Get(model);
            EnumTemplate enumTemplate = this.files.AddFile(relativePath, modelOptions)
                                            .WithName(model.FileName)
                                            .AddNamespace(modelOptions.SkipNamespace ? string.Empty : model.Namespace)
                                            .AddEnum(model.Name);

            foreach (KeyValuePair<string, object> pair in model.EnumValues)
            {
                string formattedName = Formatter.FormatProperty(pair.Key, modelOptions);
                enumTemplate.Values.Add(new EnumValueTemplate(pair.Key, Code.Local(pair.Value.ToString()), formattedName));
            }
            return enumTemplate;
        }

        protected virtual ClassTemplate WriteClass(ModelTransferObject model, string relativePath)
        {
            IOptions modelOptions = this.Options.Get(model);
            if (model.BasedOn != null && model.Language != null && modelOptions.Language != null)
            {
                this.MapType(model.Language, modelOptions.Language, model.BasedOn);
            }

            bool isInterface = model.IsInterface || modelOptions.PreferInterfaces;
            string modelNamespace = modelOptions.SkipNamespace ? string.Empty : model.Namespace;
            ClassTemplate otherClassTemplate = this.files.Where(file => file.RelativePath == relativePath
                                                                        && file.Options.Language == modelOptions.Language)
                                                   .SelectMany(file => file.Namespaces)
                                                   .SelectMany(ns => ns.Children).OfType<ClassTemplate>()
                                                   .FirstOrDefault(x => x.Namespace.Name == modelNamespace && x.Name == model.Name);
            NamespaceTemplate namespaceTemplate = otherClassTemplate?.Namespace ?? this.files.AddFile(relativePath, modelOptions)
                                                                                       .WithName(model.FileName)
                                                                                       // .WithType(isInterface ? "interface" : null)
                                                                                       .AddNamespace(modelNamespace);

            ClassTemplate classTemplate = namespaceTemplate.AddClass(model.Name, model.BasedOn?.ToTemplate())
                                                           .FormatName(modelOptions);
            if (model.BasedOn != null)
            {
                this.AddUsing(model.BasedOn, classTemplate, modelOptions);
            }

            classTemplate.IsInterface = isInterface;
            classTemplate.IsAbstract = model.IsAbstract;
            if (model is GenericModelTransferObject generic)
            {
                generic.Template.Generics.Select(x => new ClassGenericTemplate(x.Alias.Name)).ForEach(classTemplate.Generics.Add);
            }
            foreach (TypeTransferObject interFace in model.Interfaces)
            {
                if (model.Language != null && modelOptions.Language != null)
                {
                    this.MapType(model.Language, modelOptions.Language, interFace);
                }
                if (interFace.Name == model.Name)
                {
                    continue;
                }
                classTemplate.BasedOn.Add(new BaseTypeTemplate(classTemplate, interFace.ToTemplate()));
                this.AddUsing(interFace, classTemplate, modelOptions);
            }
            if (model is GenericModelTransferObject genericModel)
            {
                this.AddConstants(genericModel.Template, classTemplate);
                this.AddFields(genericModel.Template, classTemplate);
                this.AddProperties(genericModel.Template, classTemplate);
            }
            else
            {
                this.AddConstants(model, classTemplate);
                this.AddFields(model, classTemplate);
                this.AddProperties(model, classTemplate);
            }
            return classTemplate;
        }
    }
}

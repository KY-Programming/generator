using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Transfer.Writers;

public class ModelWriter : TransferWriter, ITransferWriter
{
    private readonly IList<FileTemplate> files;

    protected IEnumerable<ITransferObject> TransferObjects { get; }

    public ModelWriter(Options options, ITypeMapping typeMapping, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
        : base(options, typeMapping)
    {
        this.TransferObjects = transferObjects;
        this.files = files;
    }

    public ModelWriter FormatNames()
    {
        List<ModelTransferObject> models = this.TransferObjects.OfType<ModelTransferObject>().ToList();
        foreach (ModelTransferObject model in models)
        {
            GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
            model.Name = Formatter.FormatClass(modelOptions.Rename ?? model.OriginalName.Replace(modelOptions.ReplaceName), modelOptions)
                                  .Prefix(this.IsInterface(model, modelOptions) ? modelOptions.Formatting.InterfacePrefix : modelOptions.Formatting.ClassPrefix);
            if (!modelOptions.Language.IsGenericTypeWithSameNameAllowed)
            {
                ModelTransferObject otherModel = models.FirstOrDefault(m => m != model
                                                                            && m.Name == model.Name
                                                                            && m.Namespace == model.Namespace
                                                                            && (m as GenericModelTransferObject)?.Template != (model as GenericModelTransferObject)?.Template
                );
                if (otherModel != null && otherModel.IsGeneric && !otherModel.FromSystem)
                {
                    GeneratorOptions otherModelOptions = this.Options.Get<GeneratorOptions>(otherModel);
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
        return this;
    }

    public virtual void Write(string? relativePath = null)
    {
        foreach (ModelTransferObject model in this.TransferObjects.OfType<ModelTransferObject>())
        {
            GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
            modelOptions.ModelOutput = relativePath ?? modelOptions.ModelOutput;
            this.WriteModel(model);
        }
    }

    protected virtual void WriteModel(ModelTransferObject model)
    {
        GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
        if (modelOptions.Never)
        {
            string path = FileSystem.Combine(modelOptions.ModelOutput ?? string.Empty, model.FileName ?? model.Name);
            List<string> referencingModels = this.FindReferencingModelNames(model).ToList();
            string referencedBy = referencingModels.Count > 0
                ? $" It is referenced by {string.Join(", ", referencingModels)}."
                : "";
            throw new InvalidOperationException($"{model.Name} ({model.Namespace}) is decorated with {nameof(GenerateNeverAttribute)} and must never be generated, but it would be written to {path}.{referencedBy} Remove the reference to {model.Name} or decorate it with {nameof(GenerateIgnoreAttribute)}");
        }
        if (this.files.Any(file => file.Name == model.FileName && file.RelativePath == modelOptions.ModelOutput))
        {
            return;
        }
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
            this.WriteEnum(model);
        }
        else
        {
            this.WriteClass(model);
        }
    }

    /// <summary>
    /// Whether the model is written as interface. The one place that decides it, so the name prefix chosen in
    /// <see cref="FormatNames"/> always matches what <see cref="WriteClass"/> writes. A model with constants stays a
    /// class despite <see cref="GeneratorOptions.PreferInterfaces"/>, an interface can not carry their values
    /// </summary>
    protected virtual bool IsInterface(ModelTransferObject model, GeneratorOptions modelOptions)
    {
        return this.GetForcedInterface(model, modelOptions) ?? (model.IsInterface || modelOptions.PreferInterfaces && model.Constants.Count == 0);
    }

    /// <summary>
    /// Whether <see cref="GeneratorOptions.ForceInterface"/> or <see cref="GeneratorOptions.ForceClass"/> pin the model
    /// to an interface (<c>true</c>) or a class (<c>false</c>). <c>null</c> if neither is set and the preference decides.
    /// A force that can not be honored aborts the generation instead of silently falling back to the preference
    /// </summary>
    protected virtual bool? GetForcedInterface(ModelTransferObject model, GeneratorOptions modelOptions)
    {
        bool forceClass = modelOptions.ForceClass;
        bool forceInterface = modelOptions.ForceInterface;
        if (!forceClass && !forceInterface)
        {
            return null;
        }
        string attribute = $"{nameof(GenerateClassAttribute)}({(forceClass ? nameof(GenerateClassAttribute.ForceClass) : nameof(GenerateClassAttribute.ForceInterface))} = true)";
        string type = $"{model.OriginalName} ({model.Namespace})";
        if (forceClass && forceInterface)
        {
            throw new InvalidOperationException($"{type} is forced to be generated as class and as interface at the same time. Remove one of {nameof(GenerateClassAttribute.ForceClass)} and {nameof(GenerateClassAttribute.ForceInterface)}");
        }
        if (model.IsEnum)
        {
            throw new InvalidOperationException($"{type} is an enum and can be generated neither as class nor as interface. Remove {attribute}");
        }
        if (forceClass && model.IsInterface)
        {
            throw new InvalidOperationException($"{type} is an interface and can not be generated as class. Remove {attribute}");
        }
        if (forceInterface && model.Constants.Count > 0)
        {
            throw new InvalidOperationException($"{type} has constants and can not be generated as interface, an interface can not carry their values. Remove {attribute} or the constants");
        }
        return forceInterface;
    }

    protected virtual IEnumerable<string> FindReferencingModelNames(ModelTransferObject target)
    {
        bool ReferencesTarget(TypeTransferObject? type, HashSet<TypeTransferObject> visited)
        {
            if (type == null || !visited.Add(type))
            {
                return false;
            }
            bool isMatch = type.Type != null && target.Type != null
                ? type.Type == target.Type
                : type.Equals(target);
            if (isMatch)
            {
                return true;
            }
            return type.Generics.Any(generic => ReferencesTarget(generic.Type, visited));
        }

        IEnumerable<string> modelNames = this.TransferObjects.OfType<ModelTransferObject>()
                                              .Where(model => model != target)
                                              .Where(model => model.Properties.Any(property => ReferencesTarget(property.Type, []))
                                                              || model.Fields.Any(field => ReferencesTarget(field.Type, [])))
                                              .Select(model => model.Name);

        IEnumerable<string> actionNames = this.TransferObjects.SelectMany(transferObject => transferObject switch
                                                     {
                                                         HttpServiceTransferObject service => service.Actions.Select(action => (Owner: service.Name, Action: action)),
                                                         SignalRHubTransferObject hub => hub.Actions.Concat(hub.Events).Select(action => (Owner: hub.Name, Action: action)),
                                                         _ => []
                                                     })
                                                     .Where(entry => ReferencesTarget(entry.Action.ReturnType, [])
                                                                     || entry.Action.Parameters.Any(parameter => ReferencesTarget(parameter.Type, [])))
                                                     .Select(entry => $"{entry.Owner}.{entry.Action.Name}");

        return modelNames.Concat(actionNames).Distinct();
    }

    protected virtual EnumTemplate WriteEnum(ModelTransferObject model)
    {
        if (model.EnumValues == null)
        {
            throw new InvalidOperationException("Can not write enum without values");
        }
        GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
        EnumTemplate enumTemplate = this.files.AddFile(modelOptions.ModelOutput, modelOptions)
                                        .WithName(model.FileName)
                                        .AddNamespace(modelOptions.SkipNamespace ? string.Empty : model.Namespace)
                                        .AddEnum(model.Name)
                                        .WithComment(model.Comment);

        foreach (KeyValuePair<string, object> pair in model.EnumValues)
        {
            string formattedName = Formatter.FormatProperty(pair.Key, modelOptions);
            enumTemplate.Values.Add(new EnumValueTemplate(pair.Key, Code.Local(pair.Value.ToString()), formattedName));
        }
        return enumTemplate;
    }

    protected virtual ClassTemplate WriteClass(ModelTransferObject model)
    {
        GeneratorOptions modelOptions = this.Options.Get<GeneratorOptions>(model);
        if (model.BasedOn != null && model.Language != null && modelOptions.Language != null)
        {
            this.MapType(model.Language, modelOptions.Language, model.BasedOn);
        }

        bool isInterface = this.IsInterface(model, modelOptions);
        string modelNamespace = modelOptions.SkipNamespace ? string.Empty : model.Namespace;
        ClassTemplate otherClassTemplate = this.files.Where(file => file.RelativePath == modelOptions.ModelOutput
                                                                    && file.Options.Language == modelOptions.Language)
                                               .SelectMany(file => file.Namespaces)
                                               .SelectMany(ns => ns.Children).OfType<ClassTemplate>()
                                               .FirstOrDefault(x => x.Namespace.Name == modelNamespace && x.Name == model.Name);
        NamespaceTemplate namespaceTemplate = otherClassTemplate?.Namespace ?? this.files.AddFile(modelOptions.ModelOutput, modelOptions)
                                                                                   .WithName(model.FileName)
                                                                                   // .WithType(isInterface ? "interface" : null)
                                                                                   .AddNamespace(modelNamespace);

        ClassTemplate classTemplate = namespaceTemplate.AddClass(model.Name, model.BasedOn?.ToTemplate())
                                                       .FormatName(modelOptions)
                                                       .WithComment(model.Comment);
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

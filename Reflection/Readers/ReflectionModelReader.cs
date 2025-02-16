using System.ComponentModel;
using System.Reflection;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Models;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Language;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection.Readers;

public class ReflectionModelReader
{
    private readonly Options options;
    private readonly List<ITransferObject> transferObjects;

    public ReflectionModelReader(Options options, List<ITransferObject> transferObjects)
    {
        this.options = options;
        this.transferObjects = transferObjects;
    }

    public ModelTransferObject Read(Type type, GeneratorOptions? caller = null)
    {
        ModelTransferObject model = new();
        model.Name = model.OriginalName = type.Name;
        model.Namespace = type.Namespace;
        model.Language = ReflectionLanguage.Instance;
        model.IsNullable = !type.IsValueType;
        model.IsGeneric = type.IsGenericType;
        model.IsGenericParameter = type.IsGenericParameter;
        model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");
        if (model.IsGeneric)
        {
            model.Name = model.OriginalName = type.Name.Split('`').First();
            model = new GenericModelTransferObject(model);
        }
        ModelTransferObject? existingModel = this.transferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
        if (existingModel != null)
        {
            // TODO: Replace complete if with cached model reading after cloning of TransferModels is fixed
            if (model.IsGeneric)
            {
                GenericModelTransferObject genericModel = new(existingModel);
                ModelTransferObject modelToMap = existingModel;
                this.options.Map(genericModel, () => this.options.Get<GeneratorOptions>(modelToMap, null));
                existingModel = genericModel;
                this.ApplyGenericTemplate(type, genericModel);
                this.transferObjects.Add(existingModel);
            }
            return existingModel;
        }
        // TODO: Uncomment cached model reading after cloning of TransferModels is fixed
        // existingModel = this.environment.TransferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
        // if (existingModel != null)
        // {
        //     return this.ReadExisting(existingModel, caller);
        // }
        this.options.Map(model, () => this.options.Get(type, caller));
        GeneratorOptions typeOptions = this.options.Get(type, caller);
        Import? import = typeOptions.Imports.FirstOrDefault(import => import.Type == type);
        if (import != null)
        {
            Logger.Trace($"{type.Name} ({type.Namespace}) imported from {import.FileName}");
            return model;
        }
        if (typeOptions.Ignore)
        {
            Logger.Trace($"{type.Name} ({type.Namespace}) ignored (decorated with {nameof(GenerateIgnoreAttribute)})");
            return model;
        }
        if (type.IsGenericParameter)
        {
            return model;
        }
        if (type.IsArray)
        {
            this.ReadArray(type, model);
        }
        else if (model.IsGeneric && model.FromSystem)
        {
            this.ReadGenericFromSystem(type, model);
        }
        else if (type.IsEnum)
        {
            this.transferObjects.Add(model);
            this.ReadEnum(type, model);
        }
        else if (!model.FromSystem)
        {
            this.transferObjects.Add(model);
            this.ReadClass(type, model, caller);
        }
        if (model.Name == nameof(Nullable))
        {
            model.IsNullable = true;
        }
        return model;
    }

    private ModelTransferObject ReadExisting(ModelTransferObject model, GeneratorOptions caller)
    {
        GeneratorOptions typeOptions = this.options.Get(model, caller);
        if (model.IsGeneric)
        {
            GenericModelTransferObject genericModel = new(model);
            model = genericModel;
            // TODO: Reimplement or delete
            // if (model.Type != null)
            // {
            //     this.ApplyGenericTemplate(model.Type, genericModel);
            // }
        }
        else
        {
            model = model.Clone();
        }
        this.transferObjects.Add(model);
        this.ReadExistingMembers(model, typeOptions);
        return model;
    }

    private void ReadExistingMembers(ModelTransferObject model, GeneratorOptions caller)
    {
        model.Interfaces.OfType<ModelTransferObject>()
             .Concat(model.Generics.Select(x => x.Type))
             .Concat(model.Constants.Select(x => x.Type))
             .Concat(model.Fields.Select(x => x.Type))
             .Concat(model.Properties.Select(x => x.Type))
             .OfType<ModelTransferObject>().ForEach(x => this.ReadExisting(x, caller));
    }

    private void ReadArray(Type type, ModelTransferObject model)
    {
        // Logger.Trace($"Reflection read array {type.Name} ({type.Namespace})");
        GeneratorOptions modelOptions = this.options.Get<GeneratorOptions>(model);
        model.Name = "Array";
        model.IsGeneric = true;
        model.FromSystem = true;
        model.Generics.Add(new GenericAliasTransferObject { Type = this.Read(type.GetElementType(), modelOptions) });
    }

    private void ReadGenericFromSystem(Type type, ModelTransferObject model)
    {
        // Logger.Trace($"Reflection read generic system type {type.Name}<{string.Join(",", type.GetGenericArguments().Select(x => x.Name))}> ({type.Namespace})");
        this.ReadGenericArguments(type, model);
        this.ApplyGenericTemplate(type, (GenericModelTransferObject)model);
    }

    private void ReadEnum(Type type, ModelTransferObject model)
    {
        // Logger.Trace($"Reflection read enum {type.Name} ({type.Namespace})");
        model.IsEnum = true;
        model.EnumValues = new Dictionary<string, int>();
        Array values = Enum.GetValues(type);
        foreach (object value in values)
        {
            if (!(value is int))
            {
                //throw new InvalidOperationException($"Can not convert {value.GetType().Name} enums. Only int enums are currently implemented");
            }
        }
        foreach (int value in values.Cast<int>())
        {
            string? name = Enum.GetName(type, value);
            if (name != null)
            {
                model.EnumValues.Add(name, value);
            }
        }
    }

    private void ReadClass(Type type, ModelTransferObject model, GeneratorOptions? caller)
    {
        // Logger.Trace($"Reflection read type {type.Name} ({type.Namespace})");
        if (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType) && type.BaseType != null)
        {
            GeneratorOptions baseOptions = this.options.Get<GeneratorOptions>(type.BaseType);
            if (!baseOptions.Ignore)
            {
                model.BasedOn = this.Read(type.BaseType, caller);
            }
        }
        if (model.IsGeneric)
        {
            this.ReadGenericArguments(type, model);
        }

        model.IsInterface = type.IsInterface;
        model.IsAbstract = type.IsAbstract;
        foreach (Type interFace in type.GetInterfaces(false))
        {
            GeneratorOptions interfaceOptions = this.options.Get<GeneratorOptions>(interFace);
            if (interfaceOptions.Ignore)
            {
                continue;
            }
            ModelTransferObject interfaceTransferObject = this.Read(interFace, caller);
            if (this.transferObjects.Contains(interfaceTransferObject))
            {
                model.Interfaces.Add(interfaceTransferObject);
            }
        }
        if (model is GenericModelTransferObject genericModel)
        {
            Type genericType = type.GetGenericTypeDefinition();
            this.ReadConstants(genericType, genericModel.Template);
            this.ReadFields(genericType, genericModel.Template);
            this.ReadProperties(genericType, genericModel.Template);
            this.ApplyGenericTemplate(type, genericModel);
        }
        else
        {
            this.ReadConstants(type, model);
            this.ReadFields(type, model);
            this.ReadProperties(type, model);
        }
    }

    private void ApplyGenericTemplate(Type type, GenericModelTransferObject model)
    {
        GeneratorOptions modelOptions = this.options.Get<GeneratorOptions>(model);
        for (int index = 0; index < model.Template.Generics.Count; index++)
        {
            if (type.GenericTypeArguments.Length <= index)
            {
                break;
            }
            string alias = model.Template.Generics[index].Alias.Name;
            ModelTransferObject argument = this.Read(type.GenericTypeArguments[index], modelOptions);
            this.ApplyGenericTemplate(model, alias, argument);
        }
    }

    private void ApplyGenericTemplate(TypeTransferObject target, string alias, TypeTransferObject type)
    {
            if (target is not GenericModelTransferObject)
            {
                return;
            }
        if (target is GenericModelTransferObject genericModel && genericModel.Generics.Count == 0)
        {
            genericModel.Template.Generics.Clone().ForEach(genericModel.Generics.Add);
            genericModel.Template.Constants.Clone().ForEach(genericModel.Constants.Add);
            genericModel.Template.Fields.Clone().ForEach(genericModel.Fields.Add);
            genericModel.Template.Properties.Clone().ForEach(genericModel.Properties.Add);
        }
        GenericAliasTransferObject? aliasedGeneric = target.Generics.SingleOrDefault(x => x.Alias?.Name == alias);
        if (aliasedGeneric?.Type != null)
        {
            return;
        }
        if (aliasedGeneric != null)
        {
            aliasedGeneric.Type = type;
        }
        if (target is ModelTransferObject model)
        {
            model.Constants.ForEach(x => this.ApplyGenericTemplate(x, alias, type));
            model.Fields.ForEach(x => this.ApplyGenericTemplate(x, alias, type));
            model.Properties.ForEach(x => this.ApplyGenericTemplate(x, alias, type));
        }
    }

    private void ApplyGenericTemplate(MemberTransferObject field, string alias, TypeTransferObject type)
    {
        if (field.Type.Name == alias)
        {
            field.Type = type;
        }
        else
        {
            this.ApplyGenericTemplate(field.Type, alias, type);
        }
    }

    private void ReadProperties(Type type, ModelTransferObject model)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (PropertyInfo property in properties)
        {
            if (this.ParentHasMember(property, model.BasedOn))
            {
                Logger.Warning($"Property '{property.Name}' skipped. It is already defined on parent '{model.BasedOn.Name}' or its parent");
                continue;
            }
            GeneratorOptions propertyOptions = this.options.Get<GeneratorOptions>(property);
            if (propertyOptions.Ignore)
            {
                continue;
            }
            bool isRequired = property.CustomAttributes.Any(x => x.AttributeType.FullName == "System.ComponentModel.DataAnnotations.RequiredAttribute");
            bool isNullable = (!property.PropertyType.IsValueType && propertyOptions.Nullable
                                                                  && property.CustomAttributes.All(x => x.AttributeType.FullName != "System.Runtime.CompilerServices.NullableAttribute"))
                              || (!propertyOptions.Nullable && !property.PropertyType.IsValueType)
                              || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
            PropertyTransferObject propertyTransferObject = new()
                                                            {
                                                                Name = property.Name,
                                                                DeclaringType = model,
                                                                Attributes = property.GetCustomAttributes().ToTransferObjects().ToList(),
                                                                IsRequired = isRequired,
                                                                IsNullable = isNullable,
                                                                IsOptional = /*propertyOptions.NoOptional*/ !isRequired && isNullable,
                                                                Default = this.ReadDefaultValue(property, propertyOptions)
                                                            };
            this.options.Map(propertyTransferObject, () => this.options.Get<GeneratorOptions>(property, null));
            propertyTransferObject.Type = this.Read(property.PropertyType.IgnoreGeneric(typeof(Nullable<>)), propertyOptions);
            model.Properties.Add(propertyTransferObject);
        }
    }

    private void ReadFields(Type type, ModelTransferObject model)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (FieldInfo field in fields)
        {
            if (this.ParentHasMember(field, model.BasedOn))
            {
                Logger.Warning($"Field '{field.Name}' skipped. It is already defined on parent '{model.BasedOn.Name}' or its parent");
                continue;
            }
            GeneratorOptions fieldOptions = this.options.Get<GeneratorOptions>(field);
            if (fieldOptions.Ignore)
            {
                continue;
            }
            FieldTransferObject fieldTransferObject = new()
                                                      {
                                                          Name = field.Name,
                                                          Type = this.Read(field.FieldType, fieldOptions),
                                                          DeclaringType = model,
                                                          Attributes = field.GetCustomAttributes().ToTransferObjects().ToList(),
                                                          IsOptional = !fieldOptions.NoOptional && !field.GetCustomAttributes().Any(attribute => attribute.GetType().Name.Equals("RequiredAttribute")),
                                                          Default = this.ReadDefaultValue(field, fieldOptions)
                                                      };
            this.options.Map(fieldTransferObject, () => this.options.Get<GeneratorOptions>(field, null));
            model.Fields.Add(fieldTransferObject);
        }
    }

    private bool ParentHasMember(MemberInfo member, ModelTransferObject? parent)
    {
        if (parent == null)
        {
            return false;
        }
        if (parent.Fields.Any(field => field.Name.Equals(member.Name, StringComparison.CurrentCultureIgnoreCase))
            || parent.Properties.Any(property => property.Name.Equals(member.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            return true;
        }
        return this.ParentHasMember(member, parent.BasedOn);
    }

    private void ReadConstants(Type type, ModelTransferObject model)
    {
        FieldInfo[] constants = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (FieldInfo field in constants)
        {
            GeneratorOptions fieldOptions = this.options.Get<GeneratorOptions>(field);
            if (fieldOptions.Ignore)
            {
                continue;
            }
            object? defaultValue = this.ReadDefaultValue(field, fieldOptions);
            FieldTransferObject fieldTransferObject = new()
                                                      {
                                                          Name = field.Name,
                                                          Type = this.Read(field.FieldType, fieldOptions),
                                                          DeclaringType = model,
                                                          Default = defaultValue,
                                                          IsOptional = defaultValue == null
                                                      };
            this.options.Map(fieldTransferObject, () => this.options.Get<GeneratorOptions>(field, null));
            model.Constants.Add(fieldTransferObject);
        }
    }

    private void ReadGenericArguments(Type type, TypeTransferObject model)
    {
        model = model is GenericModelTransferObject genericModel ? genericModel.Template : model;
        Type genericType = type.GetGenericTypeDefinition();
        model.Generics.Clear();
        if (genericType is TypeInfo typeInfo)
        {
            foreach (Type alias in typeInfo.GenericTypeParameters)
            {
                model.Generics.Add(new GenericAliasTransferObject { Alias = this.Read(alias) });
            }
        }
        else
        {
            throw new InvalidOperationException("Internal Error l2sl3: Type is not a TypeInfo");
        }
    }

    private object? ReadDefaultValue(MemberInfo memberInfo, GeneratorOptions memberOptions)
    {
        object? defaultValue = memberInfo.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        if (defaultValue is Type defaultType)
        {
            return this.Read(defaultType, memberOptions);
        }
        if (defaultValue != null)
        {
            return defaultValue;
        }
        if (memberInfo is FieldInfo fieldInfo && (fieldInfo.IsLiteral || fieldInfo.IsStatic))
        {
            return memberInfo.DeclaringType?.GetField(memberInfo.Name)?.GetValue(null);
        }
        return null;
    }
}

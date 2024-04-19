using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Models;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Language;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection.Readers
{
    public class ReflectionModelReader
    {
        private readonly Options options;
        private readonly List<ITransferObject> transferObjects;
        private readonly IEnvironment environment;

        public ReflectionModelReader(Options options, List<ITransferObject> transferObjects, IEnvironment environment)
        {
            this.options = options;
            this.transferObjects = transferObjects;
            this.environment = environment;
        }

        public ModelTransferObject Read(TypeTransferObject type, IOptions caller = null)
        {
            if (type == null)
            {
                return null;
            }
            if (type.Type != null)
            {
                return this.Read(type.Type);
            }
            return new ModelTransferObject
                   {
                       Name = type.Name,
                       Namespace = type.Namespace,
                       Original = type,
                   };
        }

        public ModelTransferObject Read(Type type, IOptions caller = null)
        {
            if (type == null)
            {
                return null;
            }
            ModelTransferObject model = new() { Language = ReflectionLanguage.Instance, Type = type };
            model.Name = model.OriginalName = type.Name;
            model.Namespace = type.Namespace;
            model.IsNullable = !type.IsValueType;
            model.IsGeneric = type.IsGenericType;
            model.IsGenericParameter = type.IsGenericParameter;
            model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");

            IOptions typeOptions = this.options.Get(type, caller);
            if (model.IsGeneric)
            {
                model.Name = model.OriginalName = type.Name.Split('`').First();
                model = new GenericModelTransferObject(model);
            }
            ModelTransferObject existingModel = this.transferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
            if (existingModel != null)
            {
                // TODO: Replace complete if with cached model reading after cloning of TransferModels is fixed
                if (model.IsGeneric)
                {
                    GenericModelTransferObject genericModel = new(existingModel);
                    existingModel = genericModel;
                    this.options.Set(existingModel, typeOptions);
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
            this.options.Set(model, typeOptions);
            Import import = typeOptions.Imports.FirstOrDefault(import => import.Type == type);
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

        private ModelTransferObject ReadExisting(ModelTransferObject model, IOptions caller)
        {
            IOptions typeOptions = this.options.Get(model.Type, caller);
            if (model.IsGeneric)
            {
                GenericModelTransferObject genericModel = new(model);
                model = genericModel;
                if (model.Type != null)
                {
                    this.ApplyGenericTemplate(model.Type, genericModel);
                }
            }
            else
            {
                model = model.Clone();
            }
            if (!this.options.Contains(model))
            {
                this.options.Set(model, typeOptions);
            }
            this.transferObjects.Add(model);
            this.ReadExistingMembers(model, typeOptions);
            return model;
        }

        private void ReadExistingMembers(ModelTransferObject model, IOptions caller)
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
            IOptions modelOptions = this.options.Get(model);
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
            model.EnumValues = new Dictionary<string, object>();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.Name.Equals("value__")) 
                    continue;
                
                model.EnumValues.Add(field.Name, field.GetRawConstantValue());
            }
        }

        private void ReadClass(Type type, ModelTransferObject model, IOptions caller)
        {
            // Logger.Trace($"Reflection read type {type.Name} ({type.Namespace})");
            if (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType) && type.BaseType != null)
            {
                IOptions baseOptions = this.options.Get(type.BaseType);
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
                IOptions interfaceOptions = this.options.Get(interFace);
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
            IOptions modelOptions = this.options.Get(model);
            for (int index = 0; index < model.Template.Generics.Count; index++)
            {
                if (index >= type.GenericTypeArguments.Length)
                {
                    continue;
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
            GenericAliasTransferObject aliasedGeneric = target.Generics.SingleOrDefault(x => x.Alias?.Name == alias);
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
                IOptions propertyOptions = this.options.Get(property);
                if (propertyOptions.Ignore)
                {
                    continue;
                }
                PropertyTransferObject propertyTransferObject = new()
                                                                {
                                                                    Name = property.Name,
                                                                    Type = this.Read(propertyOptions.ReturnType, propertyOptions) ?? this.Read(property.PropertyType, propertyOptions),
                                                                    Attributes = property.GetCustomAttributes().ToTransferObjects().ToList(),
                                                                    IsAbstract = property.GetMethod?.IsAbstract ?? property.SetMethod?.IsAbstract ?? false,
                                                                    IsVirtual = property.GetMethod?.IsVirtual ?? property.SetMethod?.IsVirtual ?? false,
                                                                    IsOverwrite = property.GetMethod?.GetBaseDefinition() != property.GetMethod || property.SetMethod?.GetBaseDefinition() != property.SetMethod,
                                                                };
                model.Properties.Add(propertyTransferObject);
                this.options.Set(propertyTransferObject, propertyOptions);
            }
        }

        private void ReadFields(Type type, ModelTransferObject model)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                IOptions fieldOptions = this.options.Get(field);
                if (fieldOptions.Ignore)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new()
                                                          {
                                                              Name = field.Name,
                                                              Type = this.Read(field.FieldType, fieldOptions)
                                                          };
                model.Fields.Add(fieldTransferObject);
                this.options.Set(fieldTransferObject, fieldOptions);
            }
        }

        private void ReadConstants(Type type, ModelTransferObject model)
        {
            FieldInfo[] constants = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in constants)
            {
                IOptions fieldOptions = this.options.Get(field);
                if (fieldOptions.Ignore)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new()
                                                          {
                                                              Name = field.Name,
                                                              Type = this.Read(field.FieldType, fieldOptions),
                                                              Default = field.GetValue(null)
                                                          };
                model.Constants.Add(fieldTransferObject);
                this.options.Set(fieldTransferObject, fieldOptions);
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Language;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Reflection.Readers
{
    public class ReflectionModelReader
    {
        public ModelTransferObject Read(Type type, List<ITransferObject> transferObjects, IFromTypeOptions options = null)
        {
            bool isFromSystem = type.Namespace != null && type.Namespace.StartsWith("System");
            ModelTransferObject model = new ModelTransferObject { Language = ReflectionLanguage.Instance };
            model.FromType(type, options);
            ModelTransferObject existingModel = transferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
            if (existingModel != null)
            {
                return existingModel;
            }
            if (type.GetCustomAttributes<GenerateIgnoreAttribute>().Any())
            {
                Logger.Trace($"{type.Name} ({type.Namespace}) ignored (decorated with {nameof(GenerateIgnoreAttribute)})");
                return model;
            }
            if (type.IsArray)
            {
                this.ReadArray(type, model, transferObjects);
            }
            else if (type.IsGenericType && isFromSystem)
            {
                this.ReadGenericFromSystem(type, model, transferObjects);
            }
            else if (type.IsEnum)
            {
                transferObjects.Add(model);
                this.ReadEnum(type, model);
            }
            else if (type.ContainsGenericParameters)
            {
                model.HasUsing = false;
            }
            else if (!isFromSystem)
            {
                transferObjects.Add(model);
                this.ReadClass(type, model, transferObjects);
            }
            return model;
        }

        private void ReadArray(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read array {type.Name} ({type.Namespace})");
            model.IsGeneric = true;
            model.FromSystem = true;
            this.Read(type.GetElementType(), transferObjects);
        }

        private void ReadGenericFromSystem(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read generic system type {type.Name}<{string.Join(",", type.GetGenericArguments().Select(x => x.Name))}> ({type.Namespace})");
            model.IsGeneric = true;
            model.FromSystem = true;
            foreach (Type argument in type.GenericTypeArguments)
            {
                this.Read(argument, transferObjects);
            }
        }

        private void ReadEnum(Type type, ModelTransferObject model)
        {
            Logger.Trace($"Reflection read enum {type.Name} ({type.Namespace})");
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
                string name = Enum.GetName(type, value);
                if (name != null)
                {
                    model.EnumValues.Add(name, value);
                }
            }
        }

        private void ReadClass(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read type {type.Name} ({type.Namespace})");
            if (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType) && type.BaseType != null)
            {
                model.BasedOn = this.Read(type.BaseType, transferObjects);
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                model.IsGeneric = true;
                model.Generics.Clear();
                if (genericType is TypeInfo typeInfo)
                {
                    for (int index = 0; index < typeInfo.GenericTypeParameters.Length; index++)
                    {
                        string alias = typeInfo.GenericTypeParameters[index].Name;
                        Type argument = type.GenericTypeArguments[index];
                        model.Generics.Add(new GenericAliasTransferObject
                                           {
                                               Alias = alias,
                                               Type = this.Read(argument, transferObjects)
                                           });
                    }
                    type = genericType;
                }
                else
                {
                    throw new InvalidOperationException("Internal Error l2sl3: Type is not a TypeInfo");
                }
            }

            model.IsInterface = type.IsInterface;
            model.IsAbstract = type.IsAbstract;
            foreach (Type interFace in type.GetInterfaces(false))
            {
                ModelTransferObject interfaceTransferObject = this.Read(interFace, transferObjects);
                if (transferObjects.Contains(interfaceTransferObject))
                {
                    model.Interfaces.Add(interfaceTransferObject);
                }
            }
            FieldInfo[] constants = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in constants)
            {
                GenerateIgnoreAttribute ignoreAttribute = field.GetCustomAttribute<GenerateIgnoreAttribute>();
                if (ignoreAttribute != null)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new FieldTransferObject
                                                          {
                                                              Name = field.Name,
                                                              Type = this.Read(field.FieldType, transferObjects),
                                                              Default = field.GetValue(null)
                                                          };
                model.Constants.Add(fieldTransferObject);
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                GenerateIgnoreAttribute ignoreAttribute = field.GetCustomAttribute<GenerateIgnoreAttribute>();
                if (ignoreAttribute != null)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new FieldTransferObject
                                                          {
                                                              Name = field.Name,
                                                              Type = this.Read(field.FieldType, transferObjects)
                                                          };
                model.Fields.Add(fieldTransferObject);
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                GenerateIgnoreAttribute ignoreAttribute = property.GetCustomAttribute<GenerateIgnoreAttribute>();
                if (ignoreAttribute != null)
                {
                    continue;
                }
                PropertyTransferObject propertyTransferObject = new PropertyTransferObject
                                                                {
                                                                    Name = property.Name,
                                                                    Type = this.Read(property.PropertyType, transferObjects),
                                                                    Attributes = property.GetCustomAttributes().ToTransferObjects().ToList()
                                                                };
                model.Properties.Add(propertyTransferObject);
            }
        }
    }
}

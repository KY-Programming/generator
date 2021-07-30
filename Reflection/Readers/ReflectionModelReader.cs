using System;
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
            ReflectionOptions typeOptions = ReflectionOptions.Get(type);
            ModelTransferObject model = new ModelTransferObject { Language = ReflectionLanguage.Instance };
            model.Name = model.OriginalName = type.Name;
            model.Namespace = type.Namespace;
            model.IsNullable = !type.IsValueType;
            model.IsGeneric = type.IsGenericType;
            model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");
            if (model.IsGeneric)
            {
                model.Name = type.Name.Split('`').First();
            }
            ModelTransferObject existingModel = transferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
            if (existingModel != null)
            {
                return existingModel;
            }
            if (typeOptions.Ignore)
            {
                Logger.Trace($"{type.Name} ({type.Namespace}) ignored (decorated with {nameof(GenerateIgnoreAttribute)})");
                return model;
            }
            if (type.IsArray)
            {
                this.ReadArray(type, model, transferObjects);
            }
            else if (type.IsGenericType && model.FromSystem)
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
                model.Namespace = null;
            }
            else if (!model.FromSystem)
            {
                transferObjects.Add(model);
                this.ReadClass(type, model, transferObjects);
            }
            if (model.Name == nameof(Nullable))
            {
                model.IsNullable = true;
            }
            if (!model.FromSystem && options?.ReplaceName != null)
            {
                for (int index = 0; index < options.ReplaceName.Count; index++)
                {
                    string replaceName = options.ReplaceName[index];
                    string replaceWith = options.ReplaceWithName?.Skip(index).FirstOrDefault() ?? string.Empty;
                    model.Name = model.Name.Replace(replaceName, replaceWith);
                }
            }
            return model;
        }

        private void ReadArray(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read array {type.Name} ({type.Namespace})");
            model.Name = "Array";
            model.IsGeneric = true;
            model.HasUsing = false;
            model.Generics.Add(new GenericAliasTransferObject { Type = this.Read(type.GetElementType(), transferObjects) });
        }

        private void ReadGenericFromSystem(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read generic system type {type.Name}<{string.Join(",", type.GetGenericArguments().Select(x => x.Name))}> ({type.Namespace})");
            foreach (Type argument in type.GenericTypeArguments)
            {
                model.Generics.Add(new GenericAliasTransferObject { Type = this.Read(argument, transferObjects) });
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
                model.Generics.Clear();
                if (genericType is TypeInfo typeInfo)
                {
                    for (int index = 0; index < typeInfo.GenericTypeParameters.Length; index++)
                    {
                        Type alias = typeInfo.GenericTypeParameters[index];
                        Type argument = type.GenericTypeArguments[index];
                        model.Generics.Add(new GenericAliasTransferObject
                                           {
                                               Alias = this.Read(alias, transferObjects),
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
                ReflectionOptions fieldOptions = ReflectionOptions.Get(field);
                if (fieldOptions.Ignore)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new()
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
                ReflectionOptions fieldOptions = ReflectionOptions.Get(field);
                if (fieldOptions.Ignore)
                {
                    continue;
                }
                FieldTransferObject fieldTransferObject = new()
                                                          {
                                                              Name = field.Name,
                                                              Type = this.Read(field.FieldType, transferObjects)
                                                          };
                model.Fields.Add(fieldTransferObject);
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                ReflectionOptions propertyOptions = ReflectionOptions.Get(property);
                if (propertyOptions.Ignore)
                {
                    continue;
                }
                PropertyTransferObject propertyTransferObject = new()
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

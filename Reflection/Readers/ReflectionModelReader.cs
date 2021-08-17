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

        public ReflectionModelReader(Options options)
        {
            this.options = options;
        }

        public ModelTransferObject Read(Type type, List<ITransferObject> transferObjects, IOptions caller = null)
        {
            ModelTransferObject model = new() { Language = ReflectionLanguage.Instance };
            model.Name = model.OriginalName = type.Name;
            model.Namespace = type.Namespace;
            model.IsNullable = !type.IsValueType;
            model.IsGeneric = type.IsGenericType;
            model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");

            IOptions typeOptions = this.options.Get(type, caller);
            this.options.Set(model, typeOptions);

            if (model.IsGeneric)
            {
                model.Name = type.Name.Split('`').First();
            }
            ModelTransferObject existingModel = transferObjects.OfType<ModelTransferObject>().FirstOrDefault(entry => entry.Equals(model));
            if (existingModel != null)
            {
                if (model.IsGeneric)
                {
                    existingModel = new GenericModelTransferObject(existingModel);
                    this.ReadGenericArguments(type, existingModel, transferObjects, typeOptions);
                }
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
                this.ReadClass(type, model, transferObjects, caller);
            }
            if (model.Name == nameof(Nullable))
            {
                model.IsNullable = true;
            }
            Dictionary<string, string> replaceName = typeOptions.ReplaceName;
            if (!model.FromSystem && replaceName != null)
            {
                foreach (KeyValuePair<string, string> pair in replaceName)
                {
                    model.Name = model.Name.Replace(pair.Key, pair.Value);
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

        private void ReadClass(Type type, ModelTransferObject model, List<ITransferObject> transferObjects, IOptions caller)
        {
            Logger.Trace($"Reflection read type {type.Name} ({type.Namespace})");
            if (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType) && type.BaseType != null)
            {
                IOptions baseOptions = this.options.Get(type.BaseType);
                if (!baseOptions.Ignore)
                {
                    model.BasedOn = this.Read(type.BaseType, transferObjects, caller);
                }            }
            if (type.IsGenericType)
            {
                type = this.ReadGenericArguments(type, model, transferObjects, caller);
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
                ModelTransferObject interfaceTransferObject = this.Read(interFace, transferObjects, caller);
                if (transferObjects.Contains(interfaceTransferObject))
                {
                    model.Interfaces.Add(interfaceTransferObject);
                }
            }
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
                                                              Type = this.Read(field.FieldType, transferObjects),
                                                              Default = field.GetValue(null),
                                                          };
                model.Constants.Add(fieldTransferObject);
                this.options.Set(fieldTransferObject, fieldOptions);
            }

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
                                                              Type = this.Read(field.FieldType, transferObjects),
                                                          };
                model.Fields.Add(fieldTransferObject);
                this.options.Set(fieldTransferObject, fieldOptions);
            }
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
                                                                    Type = this.Read(property.PropertyType, transferObjects),
                                                                    Attributes = property.GetCustomAttributes().ToTransferObjects().ToList(),
                                                                };
                model.Properties.Add(propertyTransferObject);
                this.options.Set(propertyTransferObject, propertyOptions);
            }
        }

        private Type ReadGenericArguments(Type type, ModelTransferObject model, List<ITransferObject> transferObjects, IOptions caller)
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
                                           Type = this.Read(argument, transferObjects, caller)
                                       });
                }
                type = genericType;
            }
            else
            {
                throw new InvalidOperationException("Internal Error l2sl3: Type is not a TypeInfo");
            }
            return type;
        }
    }
}

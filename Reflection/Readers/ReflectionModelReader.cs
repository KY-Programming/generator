using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Reflection.Language;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.Reflection.Readers
{
    public class ReflectionModelReader
    {
        public ModelTransferObject Read(Type type, List<ITransferObject> transferObjects)
        {
            ModelTransferObject model = new ModelTransferObject { Language = ReflectionLanguage.Instance };
            model.FromType(type);
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
            else if (type.IsGenericType)
            {
                this.ReadGeneric(type, model, transferObjects);
            }
            else if (type.Namespace != null && type.Namespace.StartsWith("System"))
            {
                // Ignore system types
            }
            else if (type.IsEnum)
            {
                transferObjects.Add(model);
                this.ReadEnum(type, model);
            }
            else
            {
                transferObjects.Add(model);
                this.ReadClass(type, model, transferObjects);
            }
            return model;
        }

        private void ReadArray(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read array {type.Name} ({type.Namespace})");
            model.Name = "Array";
            model.IsGeneric = true;
            model.FromSystem = true;
            Type argument = type.GetElementType() ?? typeof(object);
            model.Generics.Add(this.Read(argument, transferObjects));
        }

        private void ReadGeneric(Type type, ModelTransferObject model, List<ITransferObject> transferObjects)
        {
            Logger.Trace($"Reflection read generic type {type.Name} ({type.Namespace})");
            //model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");
            Type typeDefinition = type.GetGenericTypeDefinition();
            //this.Read(typeDefinition, null, transferObjects);
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
            if (type.BaseType != typeof(object) && type.BaseType != null)
            {
                model.BasedOn = this.Read(type.BaseType, transferObjects);
            }

            model.IsInterface = type.IsInterface;
            model.IsAbstract = type.IsAbstract;
            model.IsGeneric = type.IsGenericType;
            foreach (Type interFace in type.GetInterfaces(false))
            {
                model.Interfaces.Add(this.Read(interFace, transferObjects));
            }
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
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
                PropertyTransferObject propertyTransferObject = new PropertyTransferObject
                                                                {
                                                                    Name = property.Name,
                                                                    Type = this.Read(property.PropertyType, transferObjects),
                                                                    CanRead = property.CanRead,
                                                                    CanWrite = property.CanWrite
                                                                };
                model.Properties.Add(propertyTransferObject);
            }
        }
    }
}
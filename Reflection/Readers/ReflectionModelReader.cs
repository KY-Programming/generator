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
        public List<ModelTransferObject> Read(Type type)
        {
            List<ModelTransferObject> list = new List<ModelTransferObject>();
            this.Read(type, null, list);
            return list;
        }

        private ModelTransferObject Read(Type type, TypeTransferObject associatedType, List<ModelTransferObject> list)
        {
            ModelTransferObject model = new ModelTransferObject { Language = ReflectionLanguage.Instance };
            model.FromType(type);
            ModelTransferObject existingModel = list.FirstOrDefault(entry => entry.Equals(model));
            if (existingModel != null)
            {
                return existingModel;
            }
            if (type.IsArray)
            {
                this.ReadArray(type, model, list);
            }
            else if (type.IsGenericType)
            {
                this.ReadGeneric(type, model, list);
            }
            else if (type.Namespace != null && type.Namespace.StartsWith("System"))
            {
                // Ignore system types
            }
            else if (type.IsEnum)
            {
                list.Add(model);
                this.ReadEnum(type, model);
            }
            else
            {
                list.Add(model);
                this.ReadClass(type, model, list);
            }

            if (associatedType != null)
            {
                associatedType.Name = model.Name;
                associatedType.FromSystem = model.FromSystem;
                associatedType.Generics.Clear();
                model.Generics.ForEach(associatedType.Generics.Add);
            }
            return model;
        }

        private void ReadArray(Type type, ModelTransferObject model, List<ModelTransferObject> list)
        {
            model.Name = "Array";
            model.IsGeneric = true;
            model.FromSystem = true;
            Type argument = type.GetElementType() ?? typeof(object);
            TypeTransferObject generic = new TypeTransferObject
                                         {
                                             Name = argument.Name,
                                             Namespace = argument.Namespace
                                         };
            this.Read(argument, generic, list);
            model.Generics.Add(generic);
        }

        private void ReadGeneric(Type type, ModelTransferObject model, List<ModelTransferObject> list)
        {
            //model.FromSystem = type.Namespace != null && type.Namespace.StartsWith("System");
            Type typeDefinition = type.GetGenericTypeDefinition();
            //this.Read(typeDefinition, null, list);
            foreach (Type argument in type.GenericTypeArguments)
            {
                this.Read(argument, null, list);
            }
        }

        private void ReadEnum(Type type, ModelTransferObject model)
        {
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

        private void ReadClass(Type type, ModelTransferObject model, List<ModelTransferObject> list)
        {
            if (type.BaseType != typeof(object) && type.BaseType != null)
            {
                model.BasedOn = this.Read(type.BaseType, null, list);
            }

            model.IsInterface = type.IsInterface;
            model.IsAbstract = type.IsAbstract;
            model.IsGeneric = type.IsGenericType;
            foreach (Type interFace in type.GetInterfaces(false))
            {
                TypeTransferObject interfaceTransfer = new TypeTransferObject { Name = interFace.Name, Namespace = interFace.Namespace };
                this.Read(interFace, interfaceTransfer, list);
                model.Interfaces.Add(interfaceTransfer);
            }
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                ModelFieldTransferObject fieldTransferObject = new ModelFieldTransferObject
                                                               {
                                                                   Name = field.Name,
                                                                   Type = new TypeTransferObject
                                                                          {
                                                                              Name = field.FieldType.Name,
                                                                              Namespace = field.FieldType.Namespace
                                                                          }
                                                               };
                model.Fields.Add(fieldTransferObject);
                this.Read(field.FieldType, fieldTransferObject.Type, list);
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                ModelPropertyTransferObject propertyTransferObject = new ModelPropertyTransferObject
                                                                     {
                                                                         Name = property.Name,
                                                                         Type = property.PropertyType.ToTransferObject(),
                                                                         CanRead = property.CanRead,
                                                                         CanWrite = property.CanWrite
                                                                     };
                model.Properties.Add(propertyTransferObject);
                this.Read(property.PropertyType, propertyTransferObject.Type, list);
            }
        }
    }
}
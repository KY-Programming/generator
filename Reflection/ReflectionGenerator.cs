using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Reflection.Configuration;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Reflection
{
    internal class ReflectionGenerator : IGenerator
    {
        public List<FileTemplate> Files { get; }

        public ReflectionGenerator()
        {
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            ReflectionConfiguration configuration = configurationBase as ReflectionConfiguration;
            if (configuration == null)
            {
                return;
            }

            foreach (ReflectionType reflectionType in configuration.Types)
            {
                Type type = this.LoadType(reflectionType);
                string nameSpace = reflectionType.SkipNamespace ? string.Empty : reflectionType.Namespace ?? type.Namespace;
                Type baseType = type.BaseType == typeof(Object) ? null : type.BaseType;
                ClassTemplate classTemplate = this.Files.AddFile(reflectionType.RelativePath)
                                                  .AddNamespace(nameSpace)
                                                  .AddClass(type.Name, Code.Type(baseType?.Name));
                if (baseType != null && nameSpace != baseType.Namespace)
                {
                    classTemplate.AddUsing(baseType.Namespace, baseType.Name, this.GetTypePath(baseType, configuration));
                }
                classTemplate.IsInterface = type.IsInterface;
                FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                {
                    if (reflectionType.FieldsToProperties)
                    {
                        this.AddProperty(field.Name, field.FieldType, classTemplate, reflectionType, nameSpace, configuration);
                    }
                    else
                    {
                        this.AddField(field.Name, field.FieldType, classTemplate, reflectionType, nameSpace, configuration);
                    }
                }
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo property in properties)
                {
                    if (reflectionType.PropertiesToFields)
                    {
                        this.AddField(property.Name, property.PropertyType, classTemplate, reflectionType, nameSpace, configuration);
                    }
                    else
                    {
                        this.AddProperty(property.Name, property.PropertyType, classTemplate, reflectionType, nameSpace, configuration, property.CanRead, property.CanWrite);
                    }
                }
            }
        }

        private Type LoadType(ReflectionType reflectionType)
        {
            string name = $"{reflectionType.Namespace}.{reflectionType.Name}";
            if (string.IsNullOrEmpty(reflectionType.Assembly))
            {
                return AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(name)).First(x => x != null);
            }
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == reflectionType.Assembly);
            if (assembly == null)
            {
                throw new NotImplementedException();
            }
            return assembly.GetType(name);
        }

        private void AddField(string name, Type type, ClassTemplate classTemplate, ReflectionType reflectionType, string nameSpace, ReflectionConfiguration configuration)
        {
            TypeTemplate fieldType = type.GetMappedType(classTemplate, reflectionType.Configuration.Language);
            this.AddUsing(type, classTemplate, nameSpace, configuration);
            classTemplate.AddField(name, fieldType).Public();
        }

        private void AddProperty(string name, Type type, ClassTemplate classTemplate, ReflectionType reflectionType, string nameSpace, ReflectionConfiguration configuration, bool canRead = true, bool canWrite = true)
        {
            TypeTemplate propertyType = type.GetMappedType(classTemplate, reflectionType.Configuration.Language);
            this.AddUsing(type, classTemplate, nameSpace, configuration);
            PropertyTemplate propertyTemplate = classTemplate.AddProperty(name, propertyType);
            propertyTemplate.HasGetter = canRead;
            propertyTemplate.HasSetter = canWrite;
        }

        private void AddUsing(Type type, ClassTemplate classTemplate, string nameSpace, ReflectionConfiguration configuration)
        {
            if (type.IsGenericType)
            {
                Type typeDefinition = type.GetGenericTypeDefinition();
                if (typeDefinition != typeof(List<>) && typeDefinition != typeof(IList<>) && typeDefinition != typeof(IEnumerable<>))
                {
                    this.AddUsing(typeDefinition, classTemplate, nameSpace, configuration);
                }
                type.GenericTypeArguments.ForEach(x => this.AddUsing(x, classTemplate, nameSpace, configuration));
            }
            if (nameSpace != type.Namespace && !type.Namespace.StartsWith("System"))
            {
                classTemplate.AddUsing(type.Namespace, type.Name, this.GetTypePath(type, configuration));
            }
        }

        private string GetTypePath(Type type, ReflectionConfiguration configuration)
        {
            return configuration.Types.FirstOrDefault(x => x.Name == type.Name)?.Using ?? ".";
        }
    }
}
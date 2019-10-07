using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Transfer;

namespace KY.Generator.Mappings
{
    internal class TypeMapping : ITypeMapping
    {
        private readonly List<TypeMappingEntry> typeMapping;

        public TypeMapping()
        {
            this.typeMapping = new List<TypeMappingEntry>();
        }

        public void Add(ILanguage fromLanguage, string fromType, ILanguage toLanguage, string toType, bool nullable = false, string nameSpace = null, bool fromSystem = false, string constructor = null)
        {
            this.typeMapping.Add(new TypeMappingEntry(fromLanguage, fromType, toLanguage, toType, nullable, nameSpace, fromSystem, constructor));
        }

        public TypeMappingEntry Get(ILanguage fromLanguage, string fromType, ILanguage toLanguage)
        {
            TypeMappingEntry mapping = this.TryGet(fromLanguage, fromType, toLanguage);
            if (mapping == null)
            {
                throw new ArgumentException($"Type '{fromType}' for language '{fromLanguage}' not mapped to '{toLanguage}'. Try updating generator or contact generator support team");
            }
            return mapping;
        }
        
        public TypeMappingEntry TryGet(ILanguage fromLanguage, string fromType, ILanguage toLanguage)
        {
            return this.typeMapping.FirstOrDefault(x => x.FromLanguage == fromLanguage && x.FromType == fromType && x.ToLanguage == toLanguage);
        }
        
        public TypeMappingEntry TryGet(ILanguage fromLanguage, Type fromType, ILanguage toLanguage)
        {
            if (fromType.IsGenericType)
            {
                string fullName = fromType.FullName?.Split('`').First();
                string types = string.Join(", ", fromType.GetGenericArguments().Select(x => x.FullName));
                return this.TryGet(fromLanguage, $"{fullName}<{types}>", toLanguage) ?? this.TryGet(fromLanguage, fullName, toLanguage);
            }
            return this.TryGet(fromLanguage, fromType.FullName, toLanguage);
        }

        public void Get(ILanguage fromLanguage, ILanguage toLanguage, TypeTransferObject type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            string typeName = string.IsNullOrEmpty(type.Namespace) ? type.Name : string.Join(".", type.Namespace, type.Name);
            TypeMappingEntry mapping = this.TryGet(fromLanguage, typeName, toLanguage);
            if (mapping != null)
            {
                type.Name = mapping.ToType;
                type.Namespace = mapping.Namespace;
                type.FromSystem = mapping.FromSystem;
            }
        }

        public ITypeMappingSyntax Map(ILanguage language, string type)
        {
            return new TypeMappingSyntax(this, language, type);
        }
    }
}
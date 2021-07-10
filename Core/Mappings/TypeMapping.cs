using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
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

        public void Add(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage, string toType, bool nullable = false, string nameSpace = null, bool fromSystem = false, string constructor = null)
        {
            this.typeMapping.Add(new TypeMappingEntry(fromLanguage, fromType, toLanguage, toType, nullable, nameSpace, fromSystem, constructor));
        }

        public TypeMappingEntry Get(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage)
        {
            TypeMappingEntry mapping = this.TryGet(fromLanguage, fromType, toLanguage);
            if (mapping == null)
            {
                throw new ArgumentException($"Type '{fromType}' for language '{fromLanguage}' not mapped to '{toLanguage}'. Try updating generator or contact generator support team");
            }
            return mapping;
        }

        public TypeMappingEntry TryGet(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage)
        {
            fromLanguage.AssertIsNotNull(nameof(fromLanguage));
            fromType.AssertIsNotNull(nameof(fromType));
            toLanguage.AssertIsNotNull(nameof(toLanguage));
            return this.typeMapping.FirstOrDefault(x => x.FromLanguage.Key == fromLanguage.Key && x.FromType == fromType && x.ToLanguage.Key == toLanguage.Key);
        }

        public TypeMappingEntry TryGet(IMappableLanguage fromLanguage, Type fromType, IMappableLanguage toLanguage)
        {
            if (fromType.IsGenericType)
            {
                string fullName = fromType.FullName?.Split('`').First();
                string types = string.Join(", ", fromType.GetGenericArguments().Select(x => x.FullName));
                return this.TryGet(fromLanguage, $"{fullName}<{types}>", toLanguage) ?? this.TryGet(fromLanguage, fullName, toLanguage);
            }
            return this.TryGet(fromLanguage, fromType.FullName, toLanguage);
        }

        public void Get(IMappableLanguage fromLanguage, IMappableLanguage toLanguage, TypeTransferObject type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            string typeName = string.IsNullOrEmpty(type.Namespace) ? type.Name : string.Join(".", type.Namespace, type.Name);
            TypeMappingEntry mapping = this.TryGet(fromLanguage, typeName, toLanguage);
            if (mapping != null)
            {
                type.Original = type.Clone();
                type.Name = mapping.ToType;
                type.Namespace = mapping.Namespace;
                type.FromSystem = mapping.FromSystem;
                type.IsNullable = type.IsNullable && mapping.Nullable;
            }
        }

        public ITypeMappingSyntax Map(IMappableLanguage language, string type)
        {
            return new TypeMappingSyntax(this, language, type);
        }
    }
}

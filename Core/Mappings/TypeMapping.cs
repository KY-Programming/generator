using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    internal class TypeMapping : ITypeMapping
    {
        private readonly List<TypeMappingEntry> typeMapping;

        public TypeMapping()
        {
            this.typeMapping = new List<TypeMappingEntry>();
        }

        public void Add(ILanguage fromLanguage, string fromType, ILanguage toLanguage, string toType, bool nullable = false, string nameSpace = null, string constructor = null)
        {
            this.typeMapping.Add(new TypeMappingEntry(fromLanguage, fromType, toLanguage, toType, nullable, nameSpace, constructor));
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

        public ITypeMappingSyntax Map(ILanguage language, string type)
        {
            return new TypeMappingSyntax(this, language, type);
        }
    }
}
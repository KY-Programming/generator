﻿using System.Linq;

namespace KY.Generator.Templates.Extensions
{
    public static class ClassTemplateExtension
    {
        public static UsingTemplate AddUsing(this ClassTemplate classTemplate, string nameSpace, string type, string path)
        {
            UsingTemplate usingTemplate = new UsingTemplate(nameSpace, type, path);
            classTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static ClassTemplate WithUsing(this ClassTemplate classTemplate, string nameSpace, string type, string path)
        {
            classTemplate.AddUsing(nameSpace, type, path);
            return classTemplate;
        }

        public static ClassTemplate WithGenericParameter(this ClassTemplate classTemplate, string name, params TypeTemplate[] constraints)
        {
            ClassGenericTemplate genericTemplate = new ClassGenericTemplate(name);
            genericTemplate.Constraints.AddRange(constraints);
            classTemplate.Generics.Add(genericTemplate);
            return classTemplate;
        }

        public static ClassTemplate Static(this ClassTemplate classTemplate)
        {
            classTemplate.IsStatic = true;
            return classTemplate;
        }

        public static ClassTemplate Abstract(this ClassTemplate classTemplate)
        {
            classTemplate.IsAbstract = true;
            return classTemplate;
        }

        public static FieldTemplate AddField(this ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            FieldTemplate field = new FieldTemplate(classTemplate, name, type);
            classTemplate.Fields.Add(field);
            return field;
        }

        public static PropertyTemplate AddProperty(this ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            PropertyTemplate property = new PropertyTemplate(classTemplate, name, type);
            classTemplate.Properties.Add(property);
            return property;
        }

        public static MethodTemplate AddMethod(this ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            MethodTemplate methodTemplate = new MethodTemplate(classTemplate, name, type);
            classTemplate.Methods.Add(methodTemplate);
            return methodTemplate;
        }

        public static ExtensionMethodTemplate AddExtensionMethod(this ClassTemplate classTemplate, string name, TypeTemplate type)
        {
            ExtensionMethodTemplate methodTemplate = new ExtensionMethodTemplate(classTemplate, name, type);
            classTemplate.Methods.Add(methodTemplate);
            return methodTemplate;
        }

        public static ClassTemplate AddClass(this ClassTemplate classTemplate, string name, TypeTemplate basedOn = null)
        {
            ClassTemplate subClass = new ClassTemplate(classTemplate, name, basedOn);
            classTemplate.Classes.Add(subClass);
            return subClass;
        }

        public static TypeTemplate ToType(this ClassTemplate classTemplate)
        {
            return classTemplate.IsGeneric() ? Code.Generic(classTemplate.Name, classTemplate.Generics.Select(x => Code.Type(x.Name)).ToArray()) : Code.Type(classTemplate.Name);
        }

        public static bool IsGeneric(this ClassTemplate classTemplate)
        {
            return classTemplate.Generics.Count > 0;
        }

        public static ClassTemplate WithCode(this ClassTemplate classTemplate, CodeFragment fragment)
        {
            classTemplate.Code = fragment;
            return classTemplate;
        }
    }
}
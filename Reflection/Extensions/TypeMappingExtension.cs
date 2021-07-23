using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Language;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map(ReflectionLanguage.Instance).To(CsharpLanguage.Instance)
                       .From("System.Array").To("Array").Namespace("System").FromSystem()
                       .From("System.Boolean").To("bool").FromSystem()
                       .From("System.Byte").To("byte").FromSystem()
                       .From("System.Char").To("char").FromSystem()
                       .From("System.DateTime").To("DateTime").Namespace("System").FromSystem()
                       .From("System.Decimal").To("decimal").FromSystem()
                       .From("System.Double").To("double").FromSystem()
                       .From("System.Guid").To("Guid").Namespace("System").FromSystem()
                       .From("System.Int16").To("short").FromSystem()
                       .From("System.Int32").To("int").FromSystem()
                       .From("System.Int64").To("long").FromSystem()
                       .From("System.Object").To("object").FromSystem()
                       .From("System.Single").To("float").FromSystem()
                       .From("System.String").To("string").Nullable().FromSystem()
                       .From("System.TimeSpan").To("TimeSpan").Namespace("System").FromSystem()
                       .From("System.UInt16").To("ushort").FromSystem()
                       .From("System.UInt32").To("uint").FromSystem()
                       .From("System.UInt64").To("ulong").FromSystem()
                       .From("System.Void").To("void").FromSystem()
                       .From("System.Collections.Generic.IList").To("IList").Namespace("System.Collections.Generic").FromSystem()
                       .From("System.Collections.Generic.List").To("List").Namespace("System.Collections.Generic").FromSystem()
                       .From("System.Collections.Generic.IEnumerable").To("IEnumerable").Namespace("System.Collections.Generic").FromSystem()
                       .From("System.Collections.Generic.Dictionary").To("Dictionary").Namespace("System.Collections.Generic").FromSystem()
                       .From("System.Collections.Generic.IDictionary").To("IDictionary").Namespace("System.Collections.Generic").FromSystem()
                       .From("System.Nullable").To("Nullable").FromSystem();

            typeMapping.Map(ReflectionLanguage.Instance).To(TypeScriptLanguage.Instance)
                       .From("System.Array").To("Array").Nullable().FromSystem()
                       .From("System.Boolean").To("boolean").Nullable().FromSystem().Default(Code.Instance.Boolean(false))
                       .From("System.Byte").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Char").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.DateTime").To("Date").Nullable().FromSystem().Default(Code.Instance.New(Code.Instance.Type("Date"), Code.Instance.Number(0)))
                       .From("System.Decimal").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Double").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Guid").To("string").Nullable().FromSystem()
                       .From("System.Int16").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Int32").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Int64").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Object").To("unknown").Nullable().FromSystem()
                       .From("System.Single").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.String").To("string").Nullable().FromSystem()
                       .From("System.TimeSpan").To("string").Nullable().FromSystem()
                       .From("System.UInt16").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.UInt32").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.UInt64").To("number").Nullable().FromSystem().Default(Code.Instance.Number(0))
                       .From("System.Void").To("void").Nullable().FromSystem()
                       .From("System.Collections.Generic.IList").To("Array").Nullable().FromSystem()
                       .From("System.Collections.Generic.List").To("Array").Nullable().FromSystem()
                       .From("System.Collections.Generic.IEnumerable").To("Array").Nullable().FromSystem()
                       .From("System.Collections.Generic.Dictionary").To("Dictionary").Nullable().FromSystem()
                       .From("System.Collections.Generic.IDictionary").To("Dictionary").Nullable().FromSystem()
                       .From("System.Nullable").To("Nullable").Nullable().FromSystem();

            return typeMapping;
        }
    }
}

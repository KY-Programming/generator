using KY.Generator.Mappings;

namespace KY.Generator.Reflection.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(Reflection.Language, "System.Boolean", Csharp.Code.Language, "bool");
            typeMapping.Add(Reflection.Language, "System.Byte", Csharp.Code.Language, "byte");
            typeMapping.Add(Reflection.Language, "System.Char", Csharp.Code.Language, "char");
            typeMapping.Add(Reflection.Language, "System.Decimal", Csharp.Code.Language, "decimal");
            typeMapping.Add(Reflection.Language, "System.Double", Csharp.Code.Language, "double");
            typeMapping.Add(Reflection.Language, "System.Int16", Csharp.Code.Language, "short");
            typeMapping.Add(Reflection.Language, "System.Int32", Csharp.Code.Language, "int");
            typeMapping.Add(Reflection.Language, "System.Int64", Csharp.Code.Language, "long");
            typeMapping.Add(Reflection.Language, "System.Single", Csharp.Code.Language, "float");
            typeMapping.Add(Reflection.Language, "System.String", Csharp.Code.Language, "string", true);
            typeMapping.Add(Reflection.Language, "System.UInt16", Csharp.Code.Language, "ushort");
            typeMapping.Add(Reflection.Language, "System.UInt32", Csharp.Code.Language, "uint");
            typeMapping.Add(Reflection.Language, "System.UInt64", Csharp.Code.Language, "ulong");

            typeMapping.Add(Reflection.Language, "System.Boolean", TypeScript.Code.Language, "boolean", true);
            typeMapping.Add(Reflection.Language, "System.Byte", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Char", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Decimal", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Double", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int16", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int32", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int64", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Single", TypeScript.Code.Language, "number");
            typeMapping.Add(Reflection.Language, "System.String", TypeScript.Code.Language, "string", true);
            typeMapping.Add(Reflection.Language, "System.UInt16", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.UInt32", TypeScript.Code.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.UInt64", TypeScript.Code.Language, "number", true);
            return typeMapping;
        }
    }
}

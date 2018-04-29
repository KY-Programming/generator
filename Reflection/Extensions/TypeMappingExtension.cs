using KY.Generator.Mappings;

namespace KY.Generator.Reflection.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(Reflection.Language, "System.Boolean", Csharp.Language, "bool");
            typeMapping.Add(Reflection.Language, "System.Byte", Csharp.Language, "byte");
            typeMapping.Add(Reflection.Language, "System.Char", Csharp.Language, "char");
            typeMapping.Add(Reflection.Language, "System.Decimal", Csharp.Language, "decimal");
            typeMapping.Add(Reflection.Language, "System.Double", Csharp.Language, "double");
            typeMapping.Add(Reflection.Language, "System.Int16", Csharp.Language, "short");
            typeMapping.Add(Reflection.Language, "System.Int32", Csharp.Language, "int");
            typeMapping.Add(Reflection.Language, "System.Int64", Csharp.Language, "long");
            typeMapping.Add(Reflection.Language, "System.Single", Csharp.Language, "float");
            typeMapping.Add(Reflection.Language, "System.String", Csharp.Language, "string", true);
            typeMapping.Add(Reflection.Language, "System.UInt16", Csharp.Language, "ushort");
            typeMapping.Add(Reflection.Language, "System.UInt32", Csharp.Language, "uint");
            typeMapping.Add(Reflection.Language, "System.UInt64", Csharp.Language, "ulong");

            typeMapping.Add(Reflection.Language, "System.Boolean", TypeScript.Language, "boolean", true);
            typeMapping.Add(Reflection.Language, "System.Byte", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Char", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Decimal", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Double", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int16", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int32", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Int64", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.Single", TypeScript.Language, "number");
            typeMapping.Add(Reflection.Language, "System.String", TypeScript.Language, "string", true);
            typeMapping.Add(Reflection.Language, "System.UInt16", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.UInt32", TypeScript.Language, "number", true);
            typeMapping.Add(Reflection.Language, "System.UInt64", TypeScript.Language, "number", true);
            return typeMapping;
        }
    }
}

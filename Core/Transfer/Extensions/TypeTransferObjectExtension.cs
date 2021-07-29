using System;
using System.Linq;
using KY.Core;
using KY.Generator.Templates;

namespace KY.Generator.Transfer.Extensions
{
    public static class TypeTransferObjectExtension
    {
        public static TypeTemplate ToTemplate(this TypeTransferObject type)
        {
            if (type == null)
            {
                throw new NullReferenceException();
            }
            if (type.Generics.Count == 0)
            {
                return new TypeTemplate(type.Name, type.Namespace, fromSystem: type.FromSystem, isNullable: type.IsNullable);
            }
            GenericTypeTemplate genericTypeTemplate = new GenericTypeTemplate(type.Name, type.Namespace, fromSystem: type.FromSystem, isNullable: type.IsNullable);
            type.Generics.Where(x => x.Type != null).ForEach(g => genericTypeTemplate.Types.Add(g.Type.ToTemplate()));
            return genericTypeTemplate;
        }

        public static void FromType(this TypeTransferObject transferObject, Type type, IFromTypeOptions options = null)
        {
            if (type == null)
            {
                return;
            }
            if (type.IsGenericType)
            {
                transferObject.Name = type.Name.Split('`').First();
                transferObject.Namespace = type.Namespace;
                foreach (Type argument in type.GetGenericArguments())
                {
                    transferObject.Generics.Add(new GenericAliasTransferObject { Type = argument.ToTransferObject(options) });
                }
            }
            else if (type.IsArray)
            {
                transferObject.Name = "Array";
                transferObject.Namespace = "System";
                Type argument = type.GetElementType() ?? typeof(object);
                transferObject.Generics.Add(new GenericAliasTransferObject { Type = argument.ToTransferObject(options) });
            }
            else if (type.ContainsGenericParameters)
            {
                transferObject.Name = type.Name;
                transferObject.HasUsing = false;
            }
            else
            {
                transferObject.Name = type.Name;
                transferObject.Namespace = type.Namespace;
                transferObject.IsNullable = !type.IsValueType;
            }
            if (transferObject.Name == nameof(Nullable))
            {
                transferObject.IsNullable = true;
            }
            if (options?.ReplaceName != null)
            {
                for (int index = 0; index < options.ReplaceName.Count; index++)
                {
                    string replaceName = options.ReplaceName[index];
                    string replaceWith = options.ReplaceWithName?.Skip(index).FirstOrDefault() ?? string.Empty;
                    transferObject.Name = transferObject.Name.Replace(replaceName, replaceWith);
                }
            }
        }
    }
}

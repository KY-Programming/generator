using System;

namespace KY.Generator.Transfer.Extensions
{
    public static class TypeExtension
    {
        public static TypeTransferObject ToTransferObject(this Type type)
        {
            TypeTransferObject transferObject = new TypeTransferObject();
            transferObject.FromType(type);
            return transferObject;
        }
    }
}
using System.Linq;
using KY.Generator.Transfer;

namespace KY.Generator.Extensions
{
    public static class TypeTransferObjectExtension
    {
        public static TypeTransferObject IgnoreNullable(this TypeTransferObject type)
        {
            if (type.Name == "Nullable")
            {
                return type.Generics.Single().Type;
            }
            return type;
        }

        public static bool IsEnumerable(this TypeTransferObject type)
        {
            return type.Name == "Array" || type.Name == "List" || type.Name == "IList" || type.Name == "IEnumerable" || type.Name == "ICollection";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection.Extensions
{
    public static class AttributeListExtension
    {
        public static IEnumerable<AttributeTransferObject> ToTransferObjects(this IEnumerable<Attribute> attributes)
        {
            return attributes.Select(attribute => new AttributeTransferObject
                                          {
                                              Name = attribute.GetType().Name,
                                              Namespace = attribute.GetType().Namespace,
                                          });
        }
    }
}

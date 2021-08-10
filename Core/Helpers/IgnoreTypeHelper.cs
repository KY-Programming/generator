using System;
using System.Collections.Generic;

namespace KY.Generator
{
    [Obsolete]
    public static class IgnoreTypeHelper
    {
        public static List<Type> IgnoredTypes { get; } = new();
    }
}

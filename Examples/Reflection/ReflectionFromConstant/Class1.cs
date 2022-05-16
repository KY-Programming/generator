﻿using KY.Generator;

namespace ReflectionFromConstant
{
    [GenerateAngularModel("Output")]
    public class Class1
    {
        public const string StringProperty = "Value-One";
        public const int NumberProperty = 7;
        public static string StaticStringProperty = "Static-Value";
        public static int StaticNumberProperty = 9;
    }
}

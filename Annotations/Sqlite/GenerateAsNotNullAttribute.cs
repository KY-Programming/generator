﻿using System;

namespace KY.Generator.Sqlite
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GenerateAsNotNullAttribute : Attribute
    { }
}
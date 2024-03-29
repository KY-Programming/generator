﻿using KY.Generator.Command;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionReadCommandParameters : GeneratorCommandParameters
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public bool OnlySubTypes { get; set; }
    }
}

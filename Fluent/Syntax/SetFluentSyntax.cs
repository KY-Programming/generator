using System;
using System.Reflection;
using KY.Generator.Helpers;
using KY.Generator.Models;

namespace KY.Generator.Syntax
{
    public class SetFluentSyntax : ISetFluentSyntax
    {
        private readonly Type type;

        public SetFluentSyntax(Type type)
        {
            this.type = type;
        }

        public ISetFluentSyntax Ignore()
        {
            IgnoreTypeHelper.IgnoredTypes.Add(this.type);
            return this;
        }
    }
}

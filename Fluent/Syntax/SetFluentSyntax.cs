using System;
using System.Reflection;

namespace KY.Generator.Syntax
{
    public class SetFluentSyntax : ISetFluentSyntax
    {
        private readonly Options options;
        private readonly IOptions optionsSet;

        private SetFluentSyntax(Options options)
        {
            this.options = options;
        }

        public SetFluentSyntax(Assembly assembly, Options options)
            : this(options)
        {
            this.optionsSet = this.options.Get(assembly);
        }

        public SetFluentSyntax(Type type, Options options)
            : this(options)
        {
            this.optionsSet = this.options.Get(type);
        }

        public SetFluentSyntax(MemberInfo member, Options options)
            : this(options)
        {
            this.optionsSet = this.options.Get(member);
        }

        public ISetFluentSyntax Strict()
        {
            this.optionsSet.Strict = true;
            return this;
        }

        public ISetFluentSyntax PropertiesToFields()
        {
            this.optionsSet.PropertiesToFields = true;
            return this;
        }

        public ISetFluentSyntax FieldsToProperties()
        {
            this.optionsSet.FieldsToProperties = true;
            return this;
        }

        public ISetFluentSyntax PreferInterfaces()
        {
            this.optionsSet.PreferInterfaces = true;
            return this;
        }

        public ISetFluentSyntax OptionalFields()
        {
            this.optionsSet.OptionalFields = true;
            return this;
        }

        public ISetFluentSyntax OptionalProperties()
        {
            this.optionsSet.OptionalProperties = true;
            return this;
        }

        public ISetFluentSyntax Ignore()
        {
            this.optionsSet.Ignore = true;
            return this;
        }

        public ISetFluentSyntax ReplaceName(string replace, string with)
        {
            this.optionsSet.ReplaceName[replace] = with;
            return this;
        }

        public ISetFluentSyntax SkipSelf()
        {
            this.optionsSet.SkipSelf = true;
            return this;
        }
    }
}

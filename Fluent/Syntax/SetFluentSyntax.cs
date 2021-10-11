using System;
using System.Reflection;

namespace KY.Generator.Syntax
{
    public class SetFluentSyntax : SetFluentSyntax<ISetFluentSyntax>, ISetFluentSyntax
    {
        public SetFluentSyntax(Assembly assembly, Options options)
            : base(assembly, options)
        { }

        public SetFluentSyntax(Type type, Options options)
            : base(type, options)
        { }

        public SetFluentSyntax(MemberInfo member, Options options)
            : base(member, options)
        { }

        protected override ISetFluentSyntax GetReturn()
        {
            return this;
        }
    }

    public abstract class SetFluentSyntax<T> : ISetFluentSyntax<T>
        where T : ISetFluentSyntax<T>
    {
        protected Options Options { get; }
        protected IOptions OptionsSet { get; }

        private SetFluentSyntax(Options options)
        {
            this.Options = options;
        }

        protected SetFluentSyntax(Assembly assembly, Options options)
            : this(options)
        {
            this.OptionsSet = this.Options.Get(assembly);
        }

        protected SetFluentSyntax(Type type, Options options)
            : this(options)
        {
            this.OptionsSet = this.Options.Get(type);
        }

        protected SetFluentSyntax(MemberInfo member, Options options)
            : this(options)
        {
            this.OptionsSet = this.Options.Get(member);
        }

        protected abstract T GetReturn();

        public T Strict()
        {
            this.OptionsSet.Strict = true;
            return this.GetReturn();
        }

        public T PropertiesToFields()
        {
            this.OptionsSet.PropertiesToFields = true;
            return this.GetReturn();
        }

        public T FieldsToProperties()
        {
            this.OptionsSet.FieldsToProperties = true;
            return this.GetReturn();
        }

        public T PreferInterfaces()
        {
            this.OptionsSet.PreferInterfaces = true;
            return this.GetReturn();
        }

        public T OptionalFields()
        {
            this.OptionsSet.OptionalFields = true;
            return this.GetReturn();
        }

        public T OptionalProperties()
        {
            this.OptionsSet.OptionalProperties = true;
            return this.GetReturn();
        }

        public T Ignore()
        {
            this.OptionsSet.Ignore = true;
            return this.GetReturn();
        }

        public T ReplaceName(string replace, string with)
        {
            this.OptionsSet.ReplaceName[replace] = with;
            return this.GetReturn();
        }

        public T OnlySubTypes()
        {
            this.OptionsSet.OnlySubTypes = true;
            return this.GetReturn();
        }

        public T FormatNames(bool value = true)
        {
            this.OptionsSet.FormatNames = value;
            return this.GetReturn();
        }
    }
}

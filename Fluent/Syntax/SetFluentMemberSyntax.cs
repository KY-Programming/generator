using System;
using System.Reflection;
using KY.Generator.Transfer;

namespace KY.Generator.Syntax
{
    public class SetFluentMemberSyntax : SetFluentSyntax<ISetMemberFluentSyntax>, ISetMemberFluentSyntax
    {
        public SetFluentMemberSyntax(Assembly assembly, Options options)
            : base(assembly, options)
        { }

        public SetFluentMemberSyntax(Type type, Options options)
            : base(type, options)
        { }

        public SetFluentMemberSyntax(MemberInfo member, Options options)
            : base(member, options)
        { }

        protected override ISetMemberFluentSyntax GetReturn()
        {
            return this;
        }

        public ISetMemberFluentSyntax Rename(string name)
        {
            this.OptionsSet.Rename = name;
            return this;
        }

        public ISetMemberFluentSyntax ReturnType(Type type)
        {
            this.OptionsSet.ReturnType = new TypeTransferObject { Name = type.Name, Namespace = type.Namespace };
            return this;
        }

        public ISetMemberFluentSyntax ReturnType(string typeName, string nameSpace, string fileName)
        {
            this.OptionsSet.ReturnType = new TypeTransferObject { Name = typeName, Namespace = nameSpace, FileName = fileName };
            return this;
        }
    }
}

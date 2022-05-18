using System;
using System.Reflection;
using KY.Generator.Transfer;

namespace KY.Generator.Syntax
{
    public class SetFluentMemberSyntax : SetFluentSyntax<ISetMemberFluentSyntax>, ISetMemberFluentSyntax, ISetMemberReturnTypeFluentSyntax
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

        public ISetMemberReturnTypeFluentSyntax ReturnType(Type type)
        {
            this.OptionsSet.ReturnType = new TypeTransferObject { Name = type.Name, Namespace = type.Namespace };
            return this;
        }

        public ISetMemberReturnTypeFluentSyntax ReturnType(string typeName)
        {
            this.OptionsSet.ReturnType = new TypeTransferObject { Name = typeName };
            return this;
        }

        public ISetMemberFluentSyntax ImportNamespace(string nameSpace)
        {
            this.OptionsSet.ReturnType.Namespace = nameSpace;
            return this;
        }

        public ISetMemberFluentSyntax ImportFile(string fileName, string type = null)
        {
            this.OptionsSet.ReturnType.FileName = fileName;
            this.OptionsSet.ReturnType.OverrideType = type;
            return this;
        }
    }
}

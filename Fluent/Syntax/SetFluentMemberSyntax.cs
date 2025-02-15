using System;
using System.Reflection;
using KY.Generator.Transfer;

namespace KY.Generator.Syntax;

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
        this.GeneratorOptions.Rename = name;
        return this;
    }

    public ISetMemberReturnTypeFluentSyntax ReturnType(Type type)
    {
        this.GeneratorOptions.ReturnType = new TypeTransferObject { Name = type.Name, Namespace = type.Namespace };
        return this;
    }

    public ISetMemberReturnTypeFluentSyntax ReturnType(string typeName)
    {
        this.GeneratorOptions.ReturnType = new TypeTransferObject { Name = typeName };
        return this;
    }

    public ISetMemberFluentSyntax ImportNamespace(string nameSpace)
    {
        this.GeneratorOptions.ReturnType.Namespace = nameSpace;
        return this;
    }

    public ISetMemberFluentSyntax ImportFile(string fileName, string type = null)
    {
        this.GeneratorOptions.ReturnType.FileName = fileName;
        this.GeneratorOptions.ReturnType.OverrideType = type;
        return this;
    }
}
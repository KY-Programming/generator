using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class TypeScriptMethodWriter : Codeable, ITemplateWriter
{
    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        MethodTemplate template = (MethodTemplate)fragment;
        if (template is TypeScriptMethodTemplate methodTemplate)
        {
            TypeScriptMethodOverloadTemplate combinedOverloadTemplate = new();
            this.Combine(combinedOverloadTemplate, methodTemplate);
            foreach (TypeScriptMethodOverloadTemplate overload in methodTemplate.Overloads)
            {
                this.Combine(combinedOverloadTemplate, overload);
                this.WriteSignature(template, overload, output);
                output.BreakLine();
            }
            this.WriteSignature(template, null, output);
            output.BreakLine();
            this.WriteSignature(template, combinedOverloadTemplate, output);
        }
        else
        {
            this.WriteSignature(template, null, output);
        }
        output.StartBlock()
              .Add(template.Code)
              .EndBlock();
    }

    private void Combine(TypeScriptMethodOverloadTemplate target, TypeScriptMethodOverloadTemplate source)
    {
        target.ReturnType = Code.UnionType(target.ReturnType, source.ReturnType);
        // TODO: Implement
        // target.Generics ??= source.Generics;
        for (int index = 0; index < source.Parameters.Count; index++)
        {
            ParameterTemplate sourceParameter = source.Parameters[index];
            if (target.Parameters.Count <= index)
            {
                target.AddParameter(sourceParameter.Type, sourceParameter.Name, sourceParameter.DefaultValue);
            }
            else
            {
                ParameterTemplate targetParameter = target.Parameters[index];
                targetParameter.Type = Code.UnionType(targetParameter.Type, sourceParameter.Type);
                targetParameter.DefaultValue ??= sourceParameter.DefaultValue;
                targetParameter.IsOptional = targetParameter.IsOptional || sourceParameter.IsOptional;
            }
        }
    }

    private void Combine(TypeScriptMethodOverloadTemplate target, TypeScriptMethodTemplate source)
    {
        target.ReturnType = Code.UnionType(target.ReturnType, source.Type);
        // TODO: Implement
        // target.Generics ??= source.Generics;
        for (int index = 0; index < source.Parameters.Count; index++)
        {
            ParameterTemplate sourceParameter = source.Parameters[index];
            if (target.Parameters.Count <= index)
            {
                target.AddParameter(sourceParameter.Type, sourceParameter.Name, sourceParameter.DefaultValue);
            }
            else
            {
                ParameterTemplate targetParameter = target.Parameters[index];
                targetParameter.Type = Code.UnionType(targetParameter.Type, sourceParameter.Type);
                targetParameter.DefaultValue ??= sourceParameter.DefaultValue;
                targetParameter.IsOptional = targetParameter.IsOptional || sourceParameter.IsOptional;
            }
        }
    }

    private void WriteSignature(MethodTemplate template, TypeScriptMethodOverloadTemplate? overload, IOutputCache output)
    {
        List<MethodGenericTemplate>? generics = overload?.Generics ?? template.Generics;
        List<ParameterTemplate> parameters = overload?.Parameters ?? template.Parameters;
        TypeTemplate? returnType = overload?.ReturnType ?? template.Type;
        output.Add(overload?.Comment ?? template.Comment)
              .Add(template.Attributes)
              .If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
              .If(template.IsStatic).Add("static ").EndIf()
              .If(template.IsOverride).Add("override ").EndIf()
              .Add(template.Name)
              .If(generics != null && generics.Count > 0).Add("<").Add(generics, ", ").Add(">").EndIf()
              .Add("(")
              .Add(parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), ", ")
              .Add(")")
              .If(returnType != null).Add(": ").Add(returnType!).EndIf();
    }
}
